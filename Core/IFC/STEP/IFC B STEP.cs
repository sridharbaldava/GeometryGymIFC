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
using System.Collections.Concurrent;
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
	public partial class IfcBeam : IfcBuiltElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcBeamTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBeamTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcBeamType : IfcBuiltElementType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBeamTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBearing : IfcBuiltElement
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() +
			(mPredefinedType == IfcBearingTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBearingTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBearingType : IfcBuiltElementType
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() +
			",." + mPredefinedType.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBearingTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBlobTexture : IfcSurfaceTexture
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",'" + mRasterFormat + "'," + ParserSTEP.BoolToString(mRasterCode); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mRasterFormat = ParserSTEP.StripString(str, ref pos, len);
			mRasterCode = ParserSTEP.StripBool(str, ref pos, len);
		}
	}
	public partial class IfcBlock : IfcCsgPrimitive3D
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleToString(mXLength) + "," + ParserSTEP.DoubleToString(mYLength) + "," + ParserSTEP.DoubleToString(mZLength); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mXLength = ParserSTEP.StripDouble(str, ref pos, len);
			mYLength = ParserSTEP.StripDouble(str, ref pos, len);
			mZLength = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBlossCurve 
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) +	ParserSTEP.DoubleToString(mQubicTerm) + "," +
			ParserSTEP.DoubleOptionalToString(mQuadraticTerm) + "," + ParserSTEP.DoubleOptionalToString(mLinearTerm);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			QubicTerm = ParserSTEP.StripDouble(str, ref pos, len);
			QuadraticTerm = ParserSTEP.StripDouble(str, ref pos, len);
			LinearTerm = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBoiler : IfcEnergyConversionDevice //IFC4  
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcBoilerTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBoilerTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcBoilerType : IfcEnergyConversionDeviceType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBoilerTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBooleanResult : IfcGeometricRepresentationItem, IfcBooleanOperand, IfcCsgSelect
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mOperator.ToString() + ".,#" + mFirstOperand.StepId + ",#" + mSecondOperand.StepId; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			Enum.TryParse<IfcBooleanOperator>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), true, out mOperator);
			mFirstOperand = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcBooleanOperand;
			mSecondOperand = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcBooleanOperand;
		}
	}
	public abstract partial class IfcBoundaryCondition : BaseClassIfc //ABSTRACT SUPERTYPE OF (ONEOF (IfcBoundaryEdgeCondition ,IfcBoundaryFaceCondition ,IfcBoundaryNodeCondition));
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (mName == "$" ? ",$" : ",'" + mName + "'"); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mName = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcBoundaryEdgeCondition : IfcBoundaryCondition
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByLengthX) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByLengthY) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByLengthZ) + "," + ParserSTEP.DoubleOptionalToString(mRotationalStiffnessByLengthX) + "," + ParserSTEP.DoubleOptionalToString(mRotationalStiffnessByLengthY) + "," + ParserSTEP.DoubleOptionalToString(mRotationalStiffnessByLengthZ); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mLinearStiffnessByLengthX = ParserSTEP.StripDouble(str, ref pos, len);
			mLinearStiffnessByLengthY = ParserSTEP.StripDouble(str, ref pos, len);
			mLinearStiffnessByLengthZ = ParserSTEP.StripDouble(str, ref pos, len);
			mRotationalStiffnessByLengthX = ParserSTEP.StripDouble(str, ref pos, len);
			mRotationalStiffnessByLengthY = ParserSTEP.StripDouble(str, ref pos, len);
			mRotationalStiffnessByLengthZ = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBoundaryFaceCondition : IfcBoundaryCondition
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByAreaX) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByAreaY) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByAreaZ); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mLinearStiffnessByAreaX = ParserSTEP.StripDouble(str, ref pos, len);
			mLinearStiffnessByAreaY = ParserSTEP.StripDouble(str, ref pos, len);
			mLinearStiffnessByAreaZ = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBoundaryNodeCondition : IfcBoundaryCondition 
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			if (release < ReleaseVersion.IFC4)
				return base.BuildStringSTEP(release) + "," + (mTranslationalStiffnessX == null ? "$" : ParserSTEP.DoubleToString(mTranslationalStiffnessX.mStiffness.mValue)) + "," +
					(mTranslationalStiffnessY == null ? "$" : ParserSTEP.DoubleToString(mTranslationalStiffnessY.mStiffness.mValue)) + "," +
					(mTranslationalStiffnessZ == null ? "$" : ParserSTEP.DoubleToString(mTranslationalStiffnessZ.mStiffness.mValue)) + "," +
					(mRotationalStiffnessX == null ? "$" : ParserSTEP.DoubleToString(mRotationalStiffnessX.mStiffness.mValue)) + "," +
					(mRotationalStiffnessY == null ? "$" : ParserSTEP.DoubleToString(mRotationalStiffnessY.mStiffness.mValue)) + "," +
					(mRotationalStiffnessZ == null ? "$" : ParserSTEP.DoubleToString(mRotationalStiffnessZ.mStiffness.mValue));
			return base.BuildStringSTEP(release) + "," + (mTranslationalStiffnessX == null ? "$" : mTranslationalStiffnessX.ToString()) + "," +
				(mTranslationalStiffnessY == null ? "$" : mTranslationalStiffnessY.ToString()) + "," +
				(mTranslationalStiffnessZ == null ? "$" : mTranslationalStiffnessZ.ToString()) + "," +
				(mRotationalStiffnessX == null ? "$" : mRotationalStiffnessX.ToString()) + "," +
				(mRotationalStiffnessY == null ? "$" : mRotationalStiffnessY.ToString()) + "," +
				(mRotationalStiffnessZ == null ? "$" : mRotationalStiffnessZ.ToString());
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mTranslationalStiffnessX = IfcTranslationalStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len), release);
			mTranslationalStiffnessY = IfcTranslationalStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len), release);
			mTranslationalStiffnessZ = IfcTranslationalStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len), release);
			mRotationalStiffnessX = IfcRotationalStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len), release);
			mRotationalStiffnessY = IfcRotationalStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len), release);
			mRotationalStiffnessZ = IfcRotationalStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len), release);
		}
	}	
	public partial class IfcBoundaryNodeConditionWarping : IfcBoundaryNodeCondition
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + mWarpingStiffness.ToString(); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mWarpingStiffness = IfcWarpingStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len));
		}
	}
	public partial class IfcBoundingBox : IfcGeometricRepresentationItem
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.LinkToString(mCorner) + "," + ParserSTEP.DoubleToString(mXDim) + "," + ParserSTEP.DoubleToString(mYDim) + "," + ParserSTEP.DoubleToString(mZDim); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mCorner = ParserSTEP.StripLink(str, ref pos, len);
			mXDim = ParserSTEP.StripDouble(str, ref pos, len);
			mYDim = ParserSTEP.StripDouble(str, ref pos, len);
			mZDim = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBoxedHalfSpace : IfcHalfSpaceSolid
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.LinkToString(mEnclosure); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary) { mEnclosure = ParserSTEP.StripLink(str, ref pos, len); }
	}
	public partial class IfcBridge : IfcFacility
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() +
			(mPredefinedType == IfcBridgeTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBridgeTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBridgePart : IfcFacilityPart
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			if (PredefinedType != null && release < ReleaseVersion.IFC4X3_RC1)
			{
				IfcBridgePartTypeEnum part = IfcBridgePartTypeEnum.NOTDEFINED;
				if (!PredefinedType.isEnumeration(ref part))
					part = IfcBridgePartTypeEnum.NOTDEFINED;
				return base.BuildStringSTEP(release) + (part == IfcBridgePartTypeEnum.NOTDEFINED ? ",$" : ",." + part.ToString() + ".");
			}
			return base.BuildStringSTEP(release);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release < ReleaseVersion.IFC4X3_RC1)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
				{
					IfcBridgePartTypeEnum partType = IfcBridgePartTypeEnum.NOTDEFINED;
					if (Enum.TryParse<IfcBridgePartTypeEnum>(s.Replace(".", ""), true, out partType))
						PredefinedType = new IfcFacilityPartTypeSelect(partType);
				}
			}
		}
	}
	public abstract partial class IfcBSplineCurve : IfcBoundedCurve //SUPERTYPE OF(IfcBSplineCurveWithKnots)
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + "," + mDegree + ",(#" + string.Join(",#", mControlPointsList.Select(x=>x.StepId.ToString())) +
				"),." + mCurveForm.ToString() + ".," + ParserIfc.LogicalToString(mClosedCurve) + "," + ParserIfc.LogicalToString(mSelfIntersect);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mDegree = int.Parse(ParserSTEP.StripField(str, ref pos, len));
			ControlPointsList.AddRange(ParserSTEP.StripListLink(str, ref pos, len).Select(x => dictionary[x] as IfcCartesianPoint));
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s[0] == '.')
				Enum.TryParse<IfcBSplineCurveForm> (s.Replace(".", ""), true, out mCurveForm);
			mClosedCurve = ParserIfc.StripLogical(str, ref pos, len);
			mSelfIntersect = ParserIfc.StripLogical(str, ref pos, len);
		}
	}
	public partial class IfcBSplineCurveWithKnots : IfcBSplineCurve
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + ",(" + string.Join(",", mKnotMultiplicities) + "),("  +
				string.Join(",", mKnots.Select(x=> ParserSTEP.DoubleToString(x))) + "),." + mKnotSpec.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mKnotMultiplicities.AddRange(ParserSTEP.StripListInt(str, ref pos, len));
			mKnots.AddRange(ParserSTEP.StripListDouble(str, ref pos, len));
			if (!Enum.TryParse<IfcKnotType>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), true, out mKnotSpec))
				mKnotSpec = IfcKnotType.UNSPECIFIED;
		}
	}
	public abstract partial class IfcBSplineSurface : IfcBoundedSurface //ABSTRACT SUPERTYPE OF	(IfcBSplineSurfaceWithKnots)
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + "," + mUDegree + "," + mVDegree + ",(" + string.Join(",", mControlPointsList.Select(x=> "(#" + string.Join(",#", x.Select(p=>p.StepId)) + ")")) + "),." + 
				mSurfaceForm.ToString() + ".," + ParserIfc.LogicalToString(mUClosed) + "," + ParserIfc.LogicalToString(mVClosed) + "," + ParserIfc.LogicalToString(mSelfIntersect);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mUDegree = int.Parse(ParserSTEP.StripField(str, ref pos, len));
			mVDegree = int.Parse(ParserSTEP.StripField(str, ref pos, len));
			foreach(List<int> ids in ParserSTEP.StripListListLink(str, ref pos, len))
				ControlPointsList.Add(new LIST<IfcCartesianPoint>(ids.Select(x => dictionary[x] as IfcCartesianPoint)));
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s[0] == '.')
				Enum.TryParse<IfcBSplineSurfaceForm>(s.Replace(".", ""), true, out mSurfaceForm);
			mUClosed = ParserIfc.StripLogical(str, ref pos, len);
			mVClosed = ParserIfc.StripLogical(str, ref pos, len);
			mSelfIntersect = ParserIfc.StripLogical(str, ref pos, len);
		}
	}
	public partial class IfcBSplineSurfaceWithKnots : IfcBSplineSurface
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			string str = base.BuildStringSTEP(release) + ",(" + ParserSTEP.IntToString(mUMultiplicities[0]);
			for (int jcounter = 1; jcounter < mUMultiplicities.Count; jcounter++)
				str += "," + ParserSTEP.IntToString(mUMultiplicities[jcounter]);
			str += "),(" + ParserSTEP.IntToString(mVMultiplicities[0]);
			for (int jcounter = 1; jcounter < mVMultiplicities.Count; jcounter++)
				str += "," + ParserSTEP.IntToString(mVMultiplicities[jcounter]);
			str += "),(" + ParserSTEP.DoubleToString(mUKnots[0]);
			for (int jcounter = 1; jcounter < mUKnots.Count; jcounter++)
				str += "," + ParserSTEP.DoubleToString(mUKnots[jcounter]);
			str += "),(" + ParserSTEP.DoubleToString(mVKnots[0]);
			for (int jcounter = 1; jcounter < mVKnots.Count; jcounter++)
				str += "," + ParserSTEP.DoubleToString(mVKnots[jcounter]);
			return str + "),." + mKnotSpec.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mUMultiplicities = ParserSTEP.StripListInt(str, ref pos, len);
			mVMultiplicities = ParserSTEP.StripListInt(str, ref pos, len);
			mUKnots = ParserSTEP.StripListDouble(str, ref pos, len);
			mVKnots = ParserSTEP.StripListDouble(str, ref pos, len);
			if (!Enum.TryParse<IfcKnotType>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), true, out mKnotSpec))
				mKnotSpec = IfcKnotType.UNSPECIFIED;
		}
	}
	public partial class IfcBuilding : IfcFacility
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleOptionalToString(mElevationOfRefHeight) + "," +
				ParserSTEP.DoubleOptionalToString(mElevationOfTerrain) + "," + ParserSTEP.ObjToLinkString(mBuildingAddress);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mElevationOfRefHeight = ParserSTEP.StripDouble(str, ref pos, len);
			mElevationOfTerrain = ParserSTEP.StripDouble(str, ref pos, len);
			mBuildingAddress = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcPostalAddress;
		}
	}
	
	/*internal class IfcBuildingElementComponent : IfcBuildingElement //IFC4 DELETED
	{
		protected static void parseFields(IfcBuildingElementComponent c, List<string> arrFields, ref int ipos) { IfcBuildingElement.parseFields(c, arrFields, ref ipos); }
	}*/
	public partial class IfcBuildingElementPart : IfcElementComponent
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcBuildingElementPartTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBuildingElementPartTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcBuildingElementPartType : IfcElementComponentType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBuildingElementPartTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBuildingElementProxy : IfcBuiltElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			string str = (mPredefinedType == IfcBuildingElementProxyTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".");
			if (mPredefinedType != IfcBuildingElementProxyTypeEnum.NOTDEFINED && release < ReleaseVersion.IFC4)
			{
				if (mPredefinedType != IfcBuildingElementProxyTypeEnum.COMPLEX && mPredefinedType != IfcBuildingElementProxyTypeEnum.ELEMENT && mPredefinedType != IfcBuildingElementProxyTypeEnum.PARTIAL)
					str = ",$";
			}
			return base.BuildStringSTEP(release) + str;
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBuildingElementProxyTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBuildingElementProxyType : IfcBuiltElementType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBuildingElementProxyTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcBuildingStorey : IfcSpatialStructureElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + formatLength(mElevation); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mElevation = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBuiltSystem : IfcSystem //IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcBuiltSystemTypeEnum.NOTDEFINED ? ",$," : ",." + mPredefinedType + ".,") + (mLongName == "$" ? "$" : "'" + mLongName + "'"));
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release > ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBuiltSystemTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
				mLongName = ParserSTEP.StripString(str, ref pos, len);
			}
		}
	}
	public partial class IfcBurner : IfcEnergyConversionDevice //IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcBurnerTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBurnerTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcBurnerType : IfcEnergyConversionDeviceType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBurnerTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
}
