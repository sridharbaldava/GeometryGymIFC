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
	public partial class IfcAirTerminal : IfcFlowTerminal
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("PredefinedType",StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Enum.TryParse<IfcAirTerminalTypeEnum>(token.Value<string>(), true, out mPredefinedType);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (mPredefinedType != IfcAirTerminalTypeEnum.NOTDEFINED)
				obj["PredefinedType"] = mPredefinedType.ToString();
		}
	}
	public partial class IfcAirTerminalType : IfcFlowTerminalType
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("PredefinedType", StringComparison.InvariantCultureIgnoreCase); 
			if (token != null)
				Enum.TryParse<IfcAirTerminalTypeEnum>(token.Value<string>(), true, out mPredefinedType);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["PredefinedType"] = mPredefinedType.ToString();
		}
	}
	public partial class IfcAlignment : IfcLinearPositioningElement //IFC4.1
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("PredefinedType", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Enum.TryParse<IfcAlignmentTypeEnum>(token.Value<string>(), true, out mPredefinedType);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (mPredefinedType != IfcAlignmentTypeEnum.NOTDEFINED)
				obj["PredefinedType"] = mPredefinedType.ToString();
		}
	}
	public partial class IfcAlignment2DCant : IfcAxisLateralInclination
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["Segments"] = new JArray(Segments.Select(x => x.getJson(this, options)));
			obj["RailHeadDistance"] = mRailHeadDistance.ToString();
		}
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			Segments.AddRange(mDatabase.extractJArray<IfcAlignment2DCantSegment>(obj.GetValue("Segments", StringComparison.InvariantCultureIgnoreCase) as JArray));
			JToken railHeadDistance = obj.GetValue("RailHeadDistance", StringComparison.InvariantCultureIgnoreCase);
			if (railHeadDistance != null)
				mRailHeadDistance = railHeadDistance.Value<double>();
		}
	}
	public abstract partial class IfcAlignment2DCantSegment : IfcAlignment2DSegment
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["StartDistAlong"] = mStartDistAlong.ToString();
			obj["HorizontalLength"] = mHorizontalLength.ToString();
			obj["StartCantLeft"] = mStartCantLeft.ToString();
			if (!double.IsNaN(mEndCantLeft))
				obj["EndCantLeft"] = mEndCantLeft.ToString();
			obj["StartCantRight"] = mStartCantRight.ToString();
			if (!double.IsNaN(mEndCantRight))
				obj["EndCantRight"] = mEndCantRight.ToString();
		}
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken startDistAlong = obj.GetValue("StartDistAlong", StringComparison.InvariantCultureIgnoreCase);
			if (startDistAlong != null)
				mStartDistAlong = startDistAlong.Value<double>();
			JToken horizontalLength = obj.GetValue("HorizontalLength", StringComparison.InvariantCultureIgnoreCase);
			if (horizontalLength != null)
				mHorizontalLength = horizontalLength.Value<double>();
			JToken startCantLeft = obj.GetValue("StartCantLeft", StringComparison.InvariantCultureIgnoreCase);
			if (startCantLeft != null)
				mStartCantLeft = startCantLeft.Value<double>();
			JToken endCantLeft = obj.GetValue("EndCantLeft", StringComparison.InvariantCultureIgnoreCase);
			if (endCantLeft != null)
				mEndCantLeft = endCantLeft.Value<double>();
			JToken startCantRight = obj.GetValue("StartCantRight", StringComparison.InvariantCultureIgnoreCase);
			if (startCantRight != null)
				mStartCantRight = startCantRight.Value<double>();
			JToken endCantRight = obj.GetValue("EndCantRight", StringComparison.InvariantCultureIgnoreCase);
			if (endCantRight != null)
				mEndCantRight = endCantRight.Value<double>();
		}
	}
	public partial class IfcAlignment2DCantSegTransition : IfcAlignment2DCantSegment
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (!double.IsNaN(mStartRadius))
				obj["StartRadius"] = mStartRadius.ToString();
			if (!double.IsNaN(mEndRadius))
				obj["EndRadius"] = mEndRadius.ToString();
			obj["IsStartRadiusCCW"] = mIsStartRadiusCCW;
			obj["IsEndRadiusCCW"] = mIsEndRadiusCCW;
			obj["TransitionCurveType"] = mTransitionCurveType.ToString();
		}
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken startRadius = obj.GetValue("StartRadius", StringComparison.InvariantCultureIgnoreCase);
			if (startRadius != null)
				mStartRadius = startRadius.Value<double>();
			JToken endRadius = obj.GetValue("EndRadius", StringComparison.InvariantCultureIgnoreCase);
			if (endRadius != null)
				mEndRadius = endRadius.Value<double>();
			JToken isStartRadiusCCW = obj.GetValue("IsStartRadiusCCW", StringComparison.InvariantCultureIgnoreCase);
			if (isStartRadiusCCW != null)
				mIsStartRadiusCCW = isStartRadiusCCW.Value<bool>();
			JToken isEndRadiusCCW = obj.GetValue("IsEndRadiusCCW", StringComparison.InvariantCultureIgnoreCase);
			if (isEndRadiusCCW != null)
				mIsEndRadiusCCW = isEndRadiusCCW.Value<bool>();
			JToken token = obj.GetValue("TransitionCurveType", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Enum.TryParse<IfcTransitionCurveType>(token.Value<string>(), true, out mTransitionCurveType);
		}
	}
	public partial class IfcAlignment2DVerSegTransition : IfcAlignment2DVerticalSegment
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (!double.IsNaN(mStartRadius))
				obj["StartRadius"] = mStartRadius.ToString();
			if (!double.IsNaN(mEndRadius))
				obj["EndRadius"] = mEndRadius.ToString();
			obj["IsStartRadiusCCW"] = mIsStartRadiusCCW;
			obj["IsEndRadiusCCW"] = mIsEndRadiusCCW;
			obj["TransitionCurveType"] = mTransitionCurveType.ToString();
		}
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken startRadius = obj.GetValue("StartRadius", StringComparison.InvariantCultureIgnoreCase);
			if (startRadius != null)
				mStartRadius = startRadius.Value<double>();
			JToken endRadius = obj.GetValue("EndRadius", StringComparison.InvariantCultureIgnoreCase);
			if (endRadius != null)
				mEndRadius = endRadius.Value<double>();
			JToken isStartRadiusCCW = obj.GetValue("IsStartRadiusCCW", StringComparison.InvariantCultureIgnoreCase);
			if (isStartRadiusCCW != null)
				mIsStartRadiusCCW = isStartRadiusCCW.Value<bool>();
			JToken isEndRadiusCCW = obj.GetValue("IsEndRadiusCCW", StringComparison.InvariantCultureIgnoreCase);
			if (isEndRadiusCCW != null)
				mIsEndRadiusCCW = isEndRadiusCCW.Value<bool>();
			JToken token = obj.GetValue("TransitionCurveType", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Enum.TryParse<IfcTransitionCurveType>(token.Value<string>(), true, out mTransitionCurveType);
		}
	}
	public partial class IfcAlignment2DHorizontal : IfcGeometricRepresentationItem //IFC4.1
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("StartDistAlong", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				StartDistAlong = token.Value<double>();
			Segments.AddRange(mDatabase.extractJArray<IfcAlignment2DHorizontalSegment>(obj.GetValue("Segments", StringComparison.InvariantCultureIgnoreCase) as JArray));
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if ((mDatabase != null && mStartDistAlong > mDatabase.Tolerance) || mStartDistAlong > 1e-5)
				obj["StartDistAlong"] = StartDistAlong;
			obj["Segments"] = new JArray(Segments.ConvertAll(x => x.getJson(this, options)));
		}
	}
	public partial class IfcAlignment2DHorizontalSegment : IfcAlignment2DSegment //IFC4.1
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("CurveGeometry", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				CurveGeometry = mDatabase.ParseJObject<IfcCurveSegment2D>(token as JObject);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["CurveGeometry"] = CurveGeometry.getJson(this, options);
		}
	}
	public abstract partial class IfcAlignment2DSegment : IfcGeometricRepresentationItem //IFC4.1
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("TangentialContinuity", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				TangentialContinuity = token.Value<bool>();
			token = obj.GetValue("StartTag", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				StartTag = token.Value<string>();
			token = obj.GetValue("EndTag", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				EndTag = token.Value<string>();
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (mTangentialContinuity != IfcLogicalEnum.UNKNOWN)
				obj["TangentialContinuity"] = TangentialContinuity;
			setAttribute(obj, "StartTag", StartTag);
			setAttribute(obj, "EndTag", EndTag);
		}
	}
	public partial class IfcAlignment2DVerSegCircularArc : IfcAlignment2DVerticalSegment  //IFC4.1
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["Radius"] = Radius;
			obj["IsConvex"] = IsConvex;
		}
	}
	public partial class IfcAlignment2DVerSegParabolicArc : IfcAlignment2DVerticalSegment  //IFC4.1
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["ParabolaConstant"] = ParabolaConstant;
			obj["IsConvex"] = IsConvex;
		}
	}
	public partial class IfcAlignment2DVertical : IfcGeometricRepresentationItem //IFC4.1
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			JArray array = new JArray();
			foreach (IfcAlignment2DVerticalSegment seg in Segments)
				array.Add(seg.getJson(this, options));
			obj["Segments"] = array;
		}
	}
	public abstract partial class IfcAlignment2DVerticalSegment : IfcAlignment2DSegment //IFC4.1
	{
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["StartDistAlong"] = StartDistAlong;
			obj["HorizontalLength"] = HorizontalLength;
			obj["StartHeight"] = StartHeight;
			obj["StartGradient"] = StartGradient;
		}
	}
	public partial class IfcAlignmentCurve : IfcBoundedCurve //IFC4.1
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("Horizontal", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Horizontal = mDatabase.ParseJObject<IfcAlignment2DHorizontal>(token as JObject);
			token = obj.GetValue("Vertical", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Vertical = mDatabase.ParseJObject<IfcAlignment2DVertical>(token as JObject);
			token = obj.GetValue("Tag", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Tag = token.Value<string>();
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (mHorizontal != null)
				obj["Horizontal"] = Horizontal.getJson(this, options);
			if (mVertical != null)
				obj["Vertical"] = Vertical.getJson(this, options);
			setAttribute(obj, "Tag", Tag);
		}
	}
	public partial class IfcAnnotation : IfcProduct
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("PredefinedType", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Enum.TryParse<IfcAnnotationTypeEnum>(token.Value<string>(), true, out mPredefinedType);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			if (mPredefinedType != IfcAnnotationTypeEnum.NOTDEFINED)
				obj["PredefinedType"] = mPredefinedType.ToString();
		}
	}
	public partial class IfcAnnotationFillArea : IfcGeometricRepresentationItem
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JObject jobj = obj.GetValue("OuterBoundary", StringComparison.InvariantCultureIgnoreCase) as JObject;
			if (jobj != null)
				OuterBoundary = mDatabase.ParseJObject<IfcCurve>(jobj);
			InnerBoundaries.AddRange(mDatabase.extractJArray<IfcCurve>(obj.GetValue("InnerBoundaries", StringComparison.InvariantCultureIgnoreCase) as JArray));
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["OuterBoundary"] = OuterBoundary.getJson(this, options);
			if(mInnerBoundaries.Count > 0)
				obj["InnerBoundaries"] = new JArray(InnerBoundaries.ToList().ConvertAll(x => x.getJson(this, options)));
		}
		
	}
	public partial class IfcApplication : BaseClassIfc
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("ApplicationDeveloper", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				ApplicationDeveloper = mDatabase.ParseJObject<IfcOrganization>(token as JObject);
			token = obj.GetValue("Version", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Version = token.Value<string>();
			token = obj.GetValue("ApplicationFullName", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				ApplicationFullName = token.Value<string>();
			token = obj.GetValue("ApplicationIdentifier", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				ApplicationIdentifier = token.Value<string>();
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			
			obj["ApplicationDeveloper"] = ApplicationDeveloper.getJson(this, options);
			obj["Version"] = Version;
			obj["ApplicationFullName"] = ApplicationFullName;
			obj["ApplicationIdentifier"] = ApplicationIdentifier;
		}
	}
	public partial class IfcAppliedValue : BaseClassIfc, IfcMetricValueSelect, IfcAppliedValueSelect, IfcResourceObjectSelect //SUPERTYPE OF(IfcCostValue);
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);

			JToken token = obj.GetValue("Name", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Name = token.Value<string>();
			token = obj.GetValue("Description", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Description = token.Value<string>();
			JObject jobj = obj.GetValue("AppliedValue", StringComparison.InvariantCultureIgnoreCase) as JObject;
			if (jobj != null)
				AppliedValue = mDatabase.ParseJObject<IfcAppliedValueSelect>(jobj);
			jobj = obj.GetValue("UnitBasis", StringComparison.InvariantCultureIgnoreCase) as JObject;
			if (jobj != null)
				UnitBasis = mDatabase.ParseJObject<IfcMeasureWithUnit>(jobj);

			mDatabase.extractJArray<IfcAppliedValue>(obj.GetValue("Components", StringComparison.InvariantCultureIgnoreCase) as JArray).ForEach(x=>addComponent(x));


			List <IfcExternalReferenceRelationship> ers = mDatabase.extractJArray<IfcExternalReferenceRelationship>(obj.GetValue("HasExternalReference", StringComparison.InvariantCultureIgnoreCase) as JArray);
			for (int icounter = 0; icounter < ers.Count; icounter++)
				ers[icounter].RelatedResourceObjects.Add(this);
			List<IfcResourceConstraintRelationship> crs = mDatabase.extractJArray<IfcResourceConstraintRelationship>(obj.GetValue("HasConstraintRelationships", StringComparison.InvariantCultureIgnoreCase) as JArray);
			for (int icounter = 0; icounter < crs.Count; icounter++)
				crs[icounter].addRelated(this);
			//todo
			token = obj.GetValue("Category", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Category = token.Value<string>();
			token = obj.GetValue("Condition", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Condition = token.Value<string>();
			token = obj.GetValue("ArithmeticOperator", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				Enum.TryParse<IfcArithmeticOperatorEnum>(token.Value<string>(),out mArithmeticOperator);

		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			setAttribute(obj, "Name", Name);
			setAttribute(obj, "Description", Description);
			if (mAppliedValue != null)
			{
				IfcValue value = mAppliedValue as IfcValue;
				if(value != null)
					obj["AppliedValue"] = DatabaseIfc.extract(value);
				else
					obj["AppliedValue"] = mAppliedValue.getJson(this, options);
			}
			if (mUnitBasis > 0)
				obj["UnitBasis"] = UnitBasis.getJson(this, options);
			//todo
			setAttribute(obj, "Category", Category);
			setAttribute(obj, "Condition", Condition);
			if (mArithmeticOperator != IfcArithmeticOperatorEnum.NONE)
				obj["ArithmeticOperator"] = ArithmeticOperator.ToString();
			if(mComponents.Count > 0)
				obj["Components"] = new JArray(Components.ToList().ConvertAll(x => x.getJson(this, options)));
			if (mHasExternalReference.Count > 0)
				obj["HasExternalReference"] = new JArray(HasExternalReference.ToList().ConvertAll(x => x.getJson(this, options)));
			if (mHasConstraintRelationships.Count > 0)
			{
				JArray array = new JArray();
				foreach (IfcResourceConstraintRelationship r in HasConstraintRelationships)
				{
					if (r.mIndex != host.mIndex)
						array.Add(r.getJson(this, options));
				}
				if(array.Count > 0)
					obj["HasConstraintRelationships"] = array;
			}
		}
	}
	public partial class IfcArbitraryClosedProfileDef : IfcProfileDef
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JObject jobj = obj.GetValue("OuterCurve", StringComparison.InvariantCultureIgnoreCase) as JObject;
			if(jobj != null)
				OuterCurve = mDatabase.ParseJObject<IfcBoundedCurve>(jobj);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["OuterCurve"] = OuterCurve.getJson(this, options);
		}
	}
	public partial class IfcAsymmetricIShapeProfileDef : IfcParameterizedProfileDef // Ifc2x3 IfcIShapeProfileDef 
	{
		//internal double mBottomFlangeEdgeRadius = double.NaN;//	:	OPTIONAL IfcNonNegativeLengthMeasure;
		//internal double mBottomFlangeSlope = double.NaN;//	:	OPTIONAL IfcPlaneAngleMeasure;
		//internal double mTopFlangeEdgeRadius = double.NaN;//	:	OPTIONAL IfcNonNegativeLengthMeasure;
		//internal double mTopFlangeSlope = double.NaN;//:	OPTIONAL IfcPlaneAngleMeasure;
		//internal double mCentreOfGravityInY;// : OPTIONAL IfcPositiveLengthMeasure IFC4 deleted
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JToken token = obj.GetValue("BottomFlangeWidth", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mBottomFlangeWidth);
			token = obj.GetValue("OverallDepth", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mOverallDepth);
			token = obj.GetValue("WebThickness", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mWebThickness);
			token = obj.GetValue("BottomFlangeThickness", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mBottomFlangeThickness);
			token = obj.GetValue("BottomFlangeFilletRadius", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mBottomFlangeFilletRadius);
			token = obj.GetValue("TopFlangeWidth", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mTopFlangeWidth);
			token = obj.GetValue("TopFlangeThickness", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mTopFlangeThickness);
			token = obj.GetValue("TopFlangeFilletRadius", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mTopFlangeFilletRadius);
			token = obj.GetValue("BottomFlangeEdgeRadius", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mBottomFlangeEdgeRadius);
			token = obj.GetValue("BottomFlangeSlope", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mBottomFlangeSlope);
			token = obj.GetValue("TopFlangeEdgeRadius", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mTopFlangeEdgeRadius);
			token = obj.GetValue("TopFlangeSlope", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mTopFlangeSlope);
			token = obj.GetValue("CentreOfGravityInY", StringComparison.InvariantCultureIgnoreCase);
			if (token != null)
				double.TryParse(token.Value<string>(), out mCentreOfGravityInY);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			obj["BottomFlangeWidth"] = formatLength(mBottomFlangeWidth);
			obj["OverallDepth"] = formatLength(mOverallDepth);
			obj["WebThickness"] = formatLength(mWebThickness);
			obj["BottomFlangeThickness"] = formatLength(mBottomFlangeThickness);
			if (!double.IsNaN(mBottomFlangeFilletRadius) && mBottomFlangeFilletRadius > 0)
				obj["BottomFlangeFilletRadius"] = formatLength(mBottomFlangeFilletRadius);
			obj["TopFlangeWidth"] = formatLength(mTopFlangeWidth);
			if (!double.IsNaN(mTopFlangeThickness) && mTopFlangeThickness > 0)
				obj["TopFlangeThickness"] = formatLength(mTopFlangeThickness);
			if (!double.IsNaN(mTopFlangeFilletRadius) && mTopFlangeFilletRadius > 0)
				obj["TopFlangeFilletRadius"] = formatLength(mTopFlangeFilletRadius);
			if (!double.IsNaN(mBottomFlangeEdgeRadius) && mBottomFlangeEdgeRadius > 0)
				obj["BottomFlangeEdgeRadius"] = formatLength(mBottomFlangeEdgeRadius);
			if (!double.IsNaN(mBottomFlangeSlope) && mBottomFlangeSlope > 0)
				obj["BottomFlangeSlope"] = mBottomFlangeSlope;
			if (!double.IsNaN(mTopFlangeEdgeRadius) && mTopFlangeEdgeRadius > 0)
				obj["TopFlangeEdgeRadius"] = formatLength(mTopFlangeEdgeRadius);
			if (!double.IsNaN(mTopFlangeSlope) && mTopFlangeSlope > 0)
				obj["TopFlangeSlope"] = mTopFlangeSlope;
			if (mDatabase.Release <= ReleaseVersion.IFC2x3)
			{
				if (!double.IsNaN(mCentreOfGravityInY) && mCentreOfGravityInY > 0)
					obj["CentreOfGravityInY"] = formatLength(mCentreOfGravityInY);
			}
		}
	}
	public partial class IfcAxis1Placement : IfcPlacement
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			Axis = mDatabase.ParseJObject<IfcDirection>(obj.GetValue("Axis", StringComparison.InvariantCultureIgnoreCase) as JObject);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			IfcDirection axis = Axis;
			if (axis != null)
				obj["Axis"] = axis.getJson(this, options);
		}
	}
	public partial class IfcAxis2Placement2D : IfcPlacement, IfcAxis2Placement
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			JObject jobj = obj.GetValue("RefDirection", StringComparison.InvariantCultureIgnoreCase) as JObject;
			if(jobj != null)
				RefDirection = mDatabase.ParseJObject<IfcDirection>(obj.GetValue("RefDirection", StringComparison.InvariantCultureIgnoreCase) as JObject);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			IfcDirection refDirection = RefDirection;
			if (refDirection != null)
				obj["RefDirection"] = refDirection.getJson(this, options);
		}
	}
	public partial class IfcAxis2Placement3D : IfcPlacement, IfcAxis2Placement
	{
		internal override void parseJObject(JObject obj)
		{
			base.parseJObject(obj);
			Axis = mDatabase.ParseJObject<IfcDirection>(obj.GetValue("Axis", StringComparison.InvariantCultureIgnoreCase) as JObject);
			RefDirection = mDatabase.ParseJObject<IfcDirection>(obj.GetValue("RefDirection", StringComparison.InvariantCultureIgnoreCase) as JObject);
		}
		protected override void setJSON(JObject obj, BaseClassIfc host, SetJsonOptions options)
		{
			base.setJSON(obj, host, options);
			IfcDirection axis = Axis, refDirection = RefDirection;

			if (axis == null)
			{
				if(refDirection != null)
				{
					obj["Axis"] = mDatabase.Factory.ZAxis.getJson(this, options);
					obj["RefDirection"] = refDirection.getJson(this, options);
				}
			}
			else if (refDirection == null)
			{
				obj["Axis"] = axis.getJson(this, options);
				obj["RefDirection"] = mDatabase.Factory.XAxis.getJson(this, options);
			}
			else if(!axis.isZAxis || !refDirection.isXAxis)
			{
				obj["Axis"] = axis.getJson(this, options);
				obj["RefDirection"] = refDirection.getJson(this, options);
			}
		}
	}
}
