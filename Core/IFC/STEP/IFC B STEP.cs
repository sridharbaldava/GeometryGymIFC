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
	public partial class IfcBeam : IfcBuildingElement
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : (mPredefinedType == IfcBeamTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBeamTypeEnum>(s.Replace(".", ""), out mPredefinedType);
			}
		}
	}
	public partial class IfcBeamType : IfcBuildingElementType
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBeamTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcBlobTexture : IfcSurfaceTexture
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",'" + mRasterFormat + "'," + ParserSTEP.BoolToString(mRasterCode); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mRasterFormat = ParserSTEP.StripString(str, ref pos, len);
			mRasterCode = ParserSTEP.StripBool(str, ref pos, len);
		}
	}
	public partial class IfcBlock : IfcCsgPrimitive3D
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.DoubleToString(mXLength) + "," + ParserSTEP.DoubleToString(mYLength) + "," + ParserSTEP.DoubleToString(mZLength); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mXLength = ParserSTEP.StripDouble(str, ref pos, len);
			mYLength = ParserSTEP.StripDouble(str, ref pos, len);
			mZLength = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBoiler : IfcEnergyConversionDevice //IFC4  
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : (mPredefinedType == IfcBoilerTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBoilerTypeEnum>(s.Replace(".", ""), out mPredefinedType);
			}
		}
	}
	public partial class IfcBoilerType : IfcEnergyConversionDeviceType
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBoilerTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcBooleanResult : IfcGeometricRepresentationItem, IfcBooleanOperand, IfcCsgSelect
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mOperator.ToString() + ".," + ParserSTEP.LinkToString(mFirstOperand) + "," + ParserSTEP.LinkToString(mSecondOperand); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			Enum.TryParse<IfcBooleanOperator>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), out mOperator);
			mFirstOperand = ParserSTEP.StripLink(str, ref pos, len);
			mSecondOperand = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	public abstract partial class IfcBoundaryCondition : BaseClassIfc //ABSTRACT SUPERTYPE OF (ONEOF (IfcBoundaryEdgeCondition ,IfcBoundaryFaceCondition ,IfcBoundaryNodeCondition));
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mName == "$" ? ",$" : ",'" + mName + "'"); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			mName = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcBoundaryEdgeCondition : IfcBoundaryCondition
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByLengthX) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByLengthY) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByLengthZ) + "," + ParserSTEP.DoubleOptionalToString(mRotationalStiffnessByLengthX) + "," + ParserSTEP.DoubleOptionalToString(mRotationalStiffnessByLengthY) + "," + ParserSTEP.DoubleOptionalToString(mRotationalStiffnessByLengthZ); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
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
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByAreaX) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByAreaY) + "," + ParserSTEP.DoubleOptionalToString(mLinearStiffnessByAreaZ); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mLinearStiffnessByAreaX = ParserSTEP.StripDouble(str, ref pos, len);
			mLinearStiffnessByAreaY = ParserSTEP.StripDouble(str, ref pos, len);
			mLinearStiffnessByAreaZ = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBoundaryNodeCondition : IfcBoundaryCondition 
	{
		protected override string BuildStringSTEP()
		{
			if (mDatabase.mRelease == ReleaseVersion.IFC2x3)
				return base.BuildStringSTEP() + "," + (mTranslationalStiffnessX == null ? "$" : ParserSTEP.DoubleToString(mTranslationalStiffnessX.mStiffness.mValue)) + "," +
					(mTranslationalStiffnessY == null ? "$" : ParserSTEP.DoubleToString(mTranslationalStiffnessY.mStiffness.mValue)) + "," +
					(mTranslationalStiffnessZ == null ? "$" : ParserSTEP.DoubleToString(mTranslationalStiffnessZ.mStiffness.mValue)) + "," +
					(mRotationalStiffnessX == null ? "$" : ParserSTEP.DoubleToString(mRotationalStiffnessX.mStiffness.mValue)) + "," +
					(mRotationalStiffnessY == null ? "$" : ParserSTEP.DoubleToString(mRotationalStiffnessY.mStiffness.mValue)) + "," +
					(mRotationalStiffnessZ == null ? "$" : ParserSTEP.DoubleToString(mRotationalStiffnessZ.mStiffness.mValue));
			return base.BuildStringSTEP() + "," + (mTranslationalStiffnessX == null ? "$" : mTranslationalStiffnessX.ToString()) + "," +
				(mTranslationalStiffnessY == null ? "$" : mTranslationalStiffnessY.ToString()) + "," +
				(mTranslationalStiffnessZ == null ? "$" : mTranslationalStiffnessZ.ToString()) + "," +
				(mRotationalStiffnessX == null ? "$" : mRotationalStiffnessX.ToString()) + "," +
				(mRotationalStiffnessY == null ? "$" : mRotationalStiffnessY.ToString()) + "," +
				(mRotationalStiffnessZ == null ? "$" : mRotationalStiffnessZ.ToString());
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
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
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + mWarpingStiffness.ToString(); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mWarpingStiffness = IfcWarpingStiffnessSelect.Parse(ParserSTEP.StripField(str, ref pos, len));
		}
	}
	public partial class IfcBoundingBox : IfcGeometricRepresentationItem
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.LinkToString(mCorner) + "," + ParserSTEP.DoubleToString(mXDim) + "," + ParserSTEP.DoubleToString(mYDim) + "," + ParserSTEP.DoubleToString(mZDim); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			mCorner = ParserSTEP.StripLink(str, ref pos, len);
			mXDim = ParserSTEP.StripDouble(str, ref pos, len);
			mYDim = ParserSTEP.StripDouble(str, ref pos, len);
			mZDim = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBoxedHalfSpace : IfcHalfSpaceSolid
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.LinkToString(mEnclosure); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len) { mEnclosure = ParserSTEP.StripLink(str, ref pos, len); }
	}
	public abstract partial class IfcBSplineCurve : IfcBoundedCurve //SUPERTYPE OF(IfcBSplineCurveWithKnots)
	{
		protected override string BuildStringSTEP()
		{
			string str = base.BuildStringSTEP() + "," + mDegree + ",(";
			str += ParserSTEP.LinkToString(mControlPointsList[0]);
			for (int icounter = 1; icounter < mControlPointsList.Count; icounter++)
				str += "," + ParserSTEP.LinkToString(mControlPointsList[icounter]);
			return str + "),." + mCurveForm.ToString() + ".," + ParserIfc.LogicalToString(mClosedCurve) + "," +
				ParserIfc.LogicalToString(mSelfIntersect);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			mDegree = int.Parse(ParserSTEP.StripField(str, ref pos, len));
			mControlPointsList = ParserSTEP.StripListLink(str, ref pos, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s[0] == '.')
				Enum.TryParse<IfcBSplineCurveForm> (s.Replace(".", ""), out mCurveForm);
			mClosedCurve = ParserIfc.StripLogical(str, ref pos, len);
			mSelfIntersect = ParserIfc.StripLogical(str, ref pos, len);
		}
	}
	public partial class IfcBSplineCurveWithKnots : IfcBSplineCurve
	{
		protected override string BuildStringSTEP()
		{
			string str = base.BuildStringSTEP() + ",(" + ParserSTEP.IntToString(mMultiplicities[0]);
			for (int jcounter = 1; jcounter < mMultiplicities.Count; jcounter++)
				str += "," + ParserSTEP.IntToString(mMultiplicities[jcounter]);
			str += "),(" + ParserSTEP.DoubleToString(mKnots[0]);
			for (int jcounter = 1; jcounter < mKnots.Count; jcounter++)
				str += "," + ParserSTEP.DoubleToString(mKnots[jcounter]);
			return str + "),." + mKnotSpec.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mMultiplicities = ParserSTEP.StripListInt(str, ref pos, len);
			mKnots = ParserSTEP.StripListDouble(str, ref pos, len);
			if (!Enum.TryParse<IfcKnotType>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), out mKnotSpec))
				mKnotSpec = IfcKnotType.UNSPECIFIED;
		}
	}
	public abstract partial class IfcBSplineSurface : IfcBoundedSurface //ABSTRACT SUPERTYPE OF	(IfcBSplineSurfaceWithKnots)
	{
		protected override string BuildStringSTEP()
		{
			List<int> cps = mControlPointsList[0];
			string str = base.BuildStringSTEP() + "," + mUDegree + "," + mVDegree + ",((" +
				ParserSTEP.LinkToString(cps[0]);
			for (int jcounter = 1; jcounter < cps.Count; jcounter++)
				str += "," + ParserSTEP.LinkToString(cps[jcounter]);
			str += ")";
			for (int icounter = 1; icounter < mControlPointsList.Count; icounter++)
			{
				cps = mControlPointsList[icounter];
				str += ",(" + ParserSTEP.LinkToString(cps[0]);
				for (int jcounter = 1; jcounter < cps.Count; jcounter++)
					str += "," + ParserSTEP.LinkToString(cps[jcounter]);
				str += ")";
			}
			return str + "),." + mSurfaceForm.ToString() + ".," + ParserIfc.LogicalToString(mUClosed) + ","
				+ ParserIfc.LogicalToString(mVClosed) + "," + ParserIfc.LogicalToString(mSelfIntersect);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			mUDegree = int.Parse(ParserSTEP.StripField(str, ref pos, len));
			mVDegree = int.Parse(ParserSTEP.StripField(str, ref pos, len));
			mControlPointsList = ParserSTEP.StripListListLink(str, ref pos, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s[0] == '.')
				Enum.TryParse<IfcBSplineSurfaceForm>(s.Replace(".", ""), out mSurfaceForm);
			mUClosed = ParserIfc.StripLogical(str, ref pos, len);
			mVClosed = ParserIfc.StripLogical(str, ref pos, len);
			mSelfIntersect = ParserIfc.StripLogical(str, ref pos, len);
		}
	}
	public partial class IfcBSplineSurfaceWithKnots : IfcBSplineSurface
	{
		protected override string BuildStringSTEP()
		{
			string str = base.BuildStringSTEP() + ",(" + ParserSTEP.IntToString(mUMultiplicities[0]);
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
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mUMultiplicities = ParserSTEP.StripListInt(str, ref pos, len);
			mVMultiplicities = ParserSTEP.StripListInt(str, ref pos, len);
			mUKnots = ParserSTEP.StripListDouble(str, ref pos, len);
			mVKnots = ParserSTEP.StripListDouble(str, ref pos, len);
			if (!Enum.TryParse<IfcKnotType>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), out mKnotSpec))
				mKnotSpec = IfcKnotType.UNSPECIFIED;
		}
	}
	public partial class IfcBuilding : IfcSpatialStructureElement
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mElevationOfRefHeight) + "," + ParserSTEP.DoubleOptionalToString(mElevationOfTerrain) + "," + ParserSTEP.LinkToString(mBuildingAddress); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mElevationOfRefHeight = ParserSTEP.StripDouble(str, ref pos, len);
			mElevationOfTerrain = ParserSTEP.StripDouble(str, ref pos, len);
			mBuildingAddress = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	
	/*internal class IfcBuildingElementComponent : IfcBuildingElement //IFC4 DELETED
	{
		protected static void parseFields(IfcBuildingElementComponent c, List<string> arrFields, ref int ipos) { IfcBuildingElement.parseFields(c, arrFields, ref ipos); }
	}*/
	public partial class IfcBuildingElementPart : IfcElementComponent
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : (mPredefinedType == IfcBuildingElementPartTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBuildingElementPartTypeEnum>(s.Replace(".",""), out mPredefinedType);
			}
		}
	}
	public partial class IfcBuildingElementPartType : IfcElementComponentType
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBuildingElementPartTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcBuildingElementProxy : IfcBuildingElement
	{
		protected override string BuildStringSTEP()
		{
			string str = (mPredefinedType == IfcBuildingElementProxyTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".");
			if (mPredefinedType != IfcBuildingElementProxyTypeEnum.NOTDEFINED && mDatabase.mRelease == ReleaseVersion.IFC2x3)
			{
				if (mPredefinedType != IfcBuildingElementProxyTypeEnum.COMPLEX && mPredefinedType != IfcBuildingElementProxyTypeEnum.ELEMENT && mPredefinedType != IfcBuildingElementProxyTypeEnum.PARTIAL)
					str = ",$";
			}
			return base.BuildStringSTEP() + str;
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBuildingElementProxyTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcBuildingElementProxyType : IfcBuildingElementType
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBuildingElementProxyTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
	public partial class IfcBuildingStorey : IfcSpatialStructureElement
	{ 
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mElevation); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			mElevation = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcBuildingSystem : IfcSystem //IFC4
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : (mLongName == "$" ? ",$,." : ",'" + mLongName + "',.") + mPredefinedType.ToString() + "."); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBuildingSystemTypeEnum>(s.Replace(".", ""), out mPredefinedType);
			mLongName = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcBurner : IfcEnergyConversionDevice //IFC4
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + (mDatabase.mRelease == ReleaseVersion.IFC2x3 ? "" : (mPredefinedType == IfcBurnerTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcBurnerTypeEnum>(s.Replace(".", ""), out mPredefinedType);
			}
		}
	}
	public partial class IfcBurnerType : IfcEnergyConversionDeviceType
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len)
		{
			base.parse(str, ref pos, release, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcBurnerTypeEnum>(s.Replace(".", ""), out mPredefinedType);
		}
	}
}
