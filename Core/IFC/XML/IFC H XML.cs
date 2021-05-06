// MIT License
// Copyright (c) 2016 Geometry Gym Pty Ltd

// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Xml;
//using System.Xml.Linq;



namespace GeometryGym.Ifc
{
	public partial class IfcHalfSpaceSolid : IfcGeometricRepresentationItem, IfcBooleanOperand /* SUPERTYPE OF (ONEOF (IfcBoxedHalfSpace ,IfcPolygonalBoundedHalfSpace)) */
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "BaseSurface") == 0)
					BaseSurface = mDatabase.ParseXml<IfcSurface>(child as XmlElement);
			}
			if (xml.HasAttribute("AgreementFlag"))
				mAgreementFlag = bool.Parse(xml.Attributes["AgreementFlag"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(BaseSurface.GetXML(xml.OwnerDocument, "BaseSurface", this, processed));
			xml.SetAttribute("AgreementFlag", mAgreementFlag.ToString().ToLower());
		}
	}
	public partial class IfcHelmertCurve
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("QubicTerm", mQuadraticTerm.ToString());
			if (!double.IsNaN(mLinearTerm))
				xml.SetAttribute("QuadraticTerm", mLinearTerm.ToString());
			if (!double.IsNaN(mConstantTerm))
				xml.SetAttribute("LinearTerm", mConstantTerm.ToString());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			string att = xml.GetAttribute("QubicTerm");
			if (!string.IsNullOrEmpty(att))
				double.TryParse(att, out mQuadraticTerm);
			att = xml.GetAttribute("QuadraticTerm");
			if (!string.IsNullOrEmpty(att))
				double.TryParse(att, out mLinearTerm);
			att = xml.GetAttribute("LinearTerm");
			if (!string.IsNullOrEmpty(att))
				double.TryParse(att, out mConstantTerm);
		}
	}
}
