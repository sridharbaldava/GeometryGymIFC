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

using Newtonsoft.Json.Linq;

namespace GeometryGym.Ifc
{
	public abstract partial class IfcValue : IfcMetricValueSelect //SELECT(IfcMeasureValue,IfcSimpleValue,IfcDerivedMeasureValue); stpentity parse method
	{
		public JObject getJson(BaseClassIfc host, BaseClassIfc.SetJsonOptions options)
		{
			return DatabaseIfc.extract(this);
		}
	}
	public partial class IfcValveType : IfcFlowControllerType
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("PredefinedType", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Enum.TryParse<IfcValveTypeEnum>(token.Value<string>(), true, out mPredefinedType);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (mPredefinedType != IfcValveTypeEnum.NOTDEFINED)
				obj["PredefinedType"] = mPredefinedType.ToString();
		}
	}
	public partial class IfcVertexPoint : IfcVertex, IfcPointOrVertexPoint
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JObject rp = obj.GetValue("VertexGeometry", StringComparison.InvariantCultureIgnoreCase) as JObject;
			if (rp != null)
				VertexGeometry = mDatabase.ParseJObject<IfcPoint>(rp);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
				obj["VertexGeometry"] = VertexGeometry.getJson(this, options);
		}
	}
	public partial class IfcVienneseBend
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["QubicTerm"] = mStartCurvature.ToString();
			if (double.IsNaN(mEndCurvature))
				obj["QuadraticTerm"] = mEndCurvature.ToString();
			if (double.IsNaN(mGravityCenterHeight))
				obj["Radius"] = mGravityCenterHeight.ToString();
		}
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("QubicTerm", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				mStartCurvature = token.Value<double>();
			token = obj.GetValue("QuadraticTerm", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				mEndCurvature = token.Value<double>();
			token = obj.GetValue("LinearTerm", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				mGravityCenterHeight = token.Value<double>();
		}
	}
}
