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
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using System.Linq;
using GeometryGym.STEP;

namespace GeometryGym.Ifc
{
	public partial class IfcValve : IfcFlowController //IFC4
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : (mPredefinedType == IfcValveTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcValveTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcValveType : IfcFlowControllerType
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcValveTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcVector : IfcGeometricRepresentationItem
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.LinkToString(mOrientation) + "," + ParserSTEP.DoubleToString(mMagnitude); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			mOrientation = ParserSTEP.StripLink(str, ref pos, len);
			mMagnitude = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcVertex : IfcTopologicalRepresentationItem //SUPERTYPE OF(IfcVertexPoint)
	{
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len) { }
	}
	public partial class IfcVertexBasedTextureMap : BaseClassIfc // DEPRECEATED IFC4
	{
		protected override string BuildStringSTEP()
		{
			string str = base.BuildStringSTEP() + ",(" + ParserSTEP.LinkToString(mTextureVertices[0]);
			for (int icounter = 1; icounter < mTextureVertices.Count; icounter++)
				str += "," + ParserSTEP.LinkToString(mTextureVertices[icounter]);
			str += "),(" + ParserSTEP.LinkToString(mTexturePoints[0]);
			for (int icounter = 1; icounter < mTexturePoints.Count; icounter++)
				str += "," + ParserSTEP.LinkToString(mTexturePoints[icounter]);
			return str;
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			mTextureVertices = ParserSTEP.StripListLink(str, ref pos, len);
			mTexturePoints = ParserSTEP.StripListLink(str, ref pos, len);
		}
	}
	public partial class IfcVertexloop : IfcLoop
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.LinkToString(mLoopVertex); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len) { mLoopVertex = ParserSTEP.StripLink(str, ref pos, str.Length); }
	}
	public partial class IfcVertexPoint : IfcVertex, IfcPointOrVertexPoint
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.LinkToString(mVertexGeometry); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len) { mVertexGeometry = ParserSTEP.StripLink(str, ref pos, str.Length); }
	}
	public partial class IfcVibrationIsolator : IfcElementComponent
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : (mPredefinedType == IfcVibrationIsolatorTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcVibrationIsolatorTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcVibrationIsolatorType : IfcElementComponentType
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcVibrationIsolatorTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcVirtualGridIntersection : BaseClassIfc
	{
		protected override string BuildStringSTEP()
		{
			string str = base.BuildStringSTEP() + ",(#" + mIntersectingAxes.Item1 + ",#" + mIntersectingAxes.Item2 + "),(";
			str += ParserSTEP.DoubleToString(mOffsetDistances.Item1) + "," + ParserSTEP.DoubleToString(mOffsetDistances.Item2);
			if (!double.IsNaN(mOffsetDistances.Item3))
				str += "," + ParserSTEP.DoubleToString(mOffsetDistances.Item3);
			str += ")";
			return str;
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			List<int> links = ParserSTEP.StripListLink(str, ref pos, len);
			mIntersectingAxes = new Tuple<int, int>(links[0], links[1]);
			List<string> lst = ParserSTEP.SplitLineFields(ParserSTEP.StripField(str,ref pos, len));
			mOffsetDistances = new Tuple<double, double, double>(ParserSTEP.ParseDouble(lst[0]), ParserSTEP.ParseDouble(lst[1]), (lst.Count > 2 ? ParserSTEP.ParseDouble(lst[2]) : double.NaN));
		}
	}
	public partial class IfcVoidingFeature : IfcFeatureElementSubtraction //IFC4
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : ",." + mPredefinedType + "."); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcVoidingFeatureTypeEnum>(s.Replace(".", ""), out mPredefinedType);
			}
		}
	}
}
