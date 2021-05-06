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
	public partial class IfcActionRequest : IfcControl
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? ",'" + mIdentification + "'" : ""); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mIdentification = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcActor : IfcObject // SUPERTYPE OF(IfcOccupant)
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",#" + mTheActor.Index; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mTheActor = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcActorSelect;
		}
	}
	public partial class IfcActorRole : BaseClassIfc, IfcResourceObjectSelect
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + (release < ReleaseVersion.IFC4 && mRole == IfcRoleEnum.COMMISSIONINGENGINEER ? "COMISSIONINGENGINEER" : mRole.ToString()) + (mUserDefinedRole == "$" ? ".,$," : ".,'" + mUserDefinedRole + "',") + (mDescription == "$" ? "$" : "'" + mDescription + "'"); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s != "$")
				mRole = (string.Compare(s, "COMISSIONINGENGINEER", true) == 0 ? IfcRoleEnum.COMMISSIONINGENGINEER : (IfcRoleEnum)Enum.Parse(typeof(IfcRoleEnum), s.Replace(".", "")));
			mUserDefinedRole = ParserSTEP.StripString(str, ref pos, len);
			mDescription = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcActuator : IfcDistributionControlElement //IFC4  
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcActuatorTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + "."));
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release > ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcActuatorTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcActuatorType : IfcDistributionControlElementType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcActuatorTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public abstract partial class IfcAddress : BaseClassIfc, IfcObjectReferenceSelect   //ABSTRACT SUPERTYPE OF(ONEOF(IfcPostalAddress, IfcTelecomAddress));
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (mPurpose == IfcAddressTypeEnum.NOTDEFINED ? ",$," : ",." + mPurpose.ToString() + ".,") + (mDescription == "$" ? "$," : "'" + mDescription + "',") + (mUserDefinedPurpose == "$" ? "$" : "'" + mUserDefinedPurpose + "'"); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAddressTypeEnum>(s.Replace(".", ""), true, out mPurpose);
			mDescription = ParserSTEP.StripString(str, ref pos, len);
			mUserDefinedPurpose = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcAdvancedBrep : IfcManifoldSolidBrep // IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return (release < ReleaseVersion.IFC4 ? "" : base.BuildStringSTEP(release)); }
	}
	public partial class IfcAdvancedBrepWithVoids : IfcAdvancedBrep
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + ",(#" + string.Join(",#", mVoids.ConvertAll(x => x.Index)) + ")";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mVoids.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcClosedShell));
		}
	}
	public partial class IfcAdvancedFace : IfcFaceSurface
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return (release < ReleaseVersion.IFC4 ? "" : base.BuildStringSTEP(release)); }
	}
	public partial class IfcAirTerminal : IfcFlowTerminal //IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcAirTerminalTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcAirTerminalTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcAirTerminalBox : IfcFlowController //IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcAirTerminalBoxTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcAirTerminalBoxTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcAirTerminalBoxType : IfcFlowControllerType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAirTerminalBoxTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAirTerminalType : IfcFlowTerminalType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAirTerminalTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAirToAirHeatRecovery : IfcEnergyConversionDevice //IFC4  
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (mPredefinedType == IfcAirToAirHeatRecoveryTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + "."); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcAirToAirHeatRecoveryTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcAirToAirHeatRecoveryType : IfcEnergyConversionDeviceType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAirToAirHeatRecoveryTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAlarm : IfcDistributionControlElement //IFC4  
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcAlarmTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcAlarmTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcAlarmType : IfcDistributionControlElementType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAlarmTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAlignment : IfcLinearPositioningElement //IFC4.1
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + (mPredefinedType == IfcAlignmentTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAlignmentTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAlignment2DCant : IfcAxisLateralInclination
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + ",(#" + string.Join(",#", mSegments.ConvertAll(x => x.StepId.ToString())) + ")" + "," +
			ParserSTEP.DoubleOptionalToString(mRailHeadDistance);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			Segments.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcAlignment2DCantSegment));
			RailHeadDistance = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public abstract partial class IfcAlignment2DCantSegment : IfcAlignment2DSegment
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mStartDistAlong) + "," +
			ParserSTEP.DoubleOptionalToString(mHorizontalLength) + "," + ParserSTEP.DoubleOptionalToString(mStartCantLeft) + "," +
			ParserSTEP.DoubleOptionalToString(mStartCantRight); 
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			StartDistAlong = ParserSTEP.StripDouble(str, ref pos, len);
			HorizontalLength = ParserSTEP.StripDouble(str, ref pos, len);
			StartCantLeft = ParserSTEP.StripDouble(str, ref pos, len);
			StartCantRight = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcAlignment2DCantSegTransition : IfcAlignment2DCantSegment
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mEndCantLeft) + "," + ParserSTEP.DoubleOptionalToString(mEndCantRight);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			EndCantLeft = ParserSTEP.StripDouble(str, ref pos, len);
			EndCantRight = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcAlignment2DCantSegTransitionNonLinear : IfcAlignment2DCantSegTransition
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mStartRadius) + "," +
			ParserSTEP.DoubleOptionalToString(mEndRadius) + (mIsStartRadiusCCW ? ",.T." : ",.F.") +
			(mIsEndRadiusCCW ? ",.T." : ",.F.") + ",." + mTransitionCurveType.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			StartRadius = ParserSTEP.StripDouble(str, ref pos, len);
			EndRadius = ParserSTEP.StripDouble(str, ref pos, len);
			IsStartRadiusCCW = ParserSTEP.StripBool(str, ref pos, len);
			IsEndRadiusCCW = ParserSTEP.StripBool(str, ref pos, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcTransitionCurveType>(s.Replace(".", ""), true, out mTransitionCurveType);
		}
	}
	public partial class IfcAlignment2DHorizontal : IfcGeometricRepresentationItem //IFC4.1
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + "," + StepOptionalLengthString(mStartDistAlong) + ",(#" + string.Join(",#", mSegments.ConvertAll(x => x.Index)) + ")";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			mStartDistAlong = ParserSTEP.StripDouble(str, ref pos, len);
			mSegments.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcAlignment2DHorizontalSegment));
		}
	}
	public partial class IfcAlignment2DHorizontalSegment : IfcAlignment2DSegment //IFC4.1
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + ",#" + mCurveGeometry.Index; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			CurveGeometry = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCurveSegment2D;
		}
	}
	public abstract partial class IfcAlignment2DSegment : IfcGeometricRepresentationItem //IFC4.1 ABSTRACT SUPERTYPE OF(ONEOF(IfcAlignment2DHorizontalSegment, IfcAlignment2DVerticalSegment))
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + (mTangentialContinuity == IfcLogicalEnum.UNKNOWN ? ",$," : (mTangentialContinuity == IfcLogicalEnum.TRUE ? ",.T.," : ",.F.,")) +
				(mStartTag == "$" ? "$," : "'" + mStartTag + "',") + (mEndTag == "$" ? "$" : "'" + mEndTag + "'");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			mTangentialContinuity = ParserIfc.StripLogical(str, ref pos, len);
			mStartTag = ParserSTEP.StripString(str, ref pos, len);
			mEndTag = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcAlignment2DVerSegCircularArc : IfcAlignment2DVerticalSegment  //IFC4.1
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + formatLength(mRadius) + "," + ParserSTEP.BoolToString(mIsConvex); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mRadius = ParserSTEP.StripDouble(str, ref pos, len);
			mIsConvex = ParserSTEP.StripBool(str, ref pos, len);
		}
	}
	public partial class IfcAlignment2DVerSegParabolicArc : IfcAlignment2DVerticalSegment  //IFC4.1
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + ParserSTEP.DoubleToString(mParabolaConstant) + "," + ParserSTEP.BoolToString(mIsConvex); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mParabolaConstant = ParserSTEP.StripDouble(str, ref pos, len);
			mIsConvex = ParserSTEP.StripBool(str, ref pos, len);
		}
	}
	public partial class IfcAlignment2DVertical : IfcGeometricRepresentationItem //IFC4.1
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + ",(#" + string.Join(",#", mSegments.ConvertAll(x => x.Index)) + ")";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary) { mSegments.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcAlignment2DVerticalSegment)); }
	}
	public abstract partial class IfcAlignment2DVerticalSegment : IfcAlignment2DSegment //IFC4.1
	{
		protected override string BuildStringSTEP() { return base.BuildStringSTEP() + "," + formatLength(mStartDistAlong) + "," + formatLength(mHorizontalLength) + "," + formatLength(mStartHeight) + "," + ParserSTEP.DoubleToString(mStartGradient); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mStartDistAlong = ParserSTEP.StripDouble(str, ref pos, len);
			mHorizontalLength = ParserSTEP.StripDouble(str, ref pos, len);
			mStartHeight = ParserSTEP.StripDouble(str, ref pos, len);
			mStartGradient = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcAlignmentCant : IfcLinearElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleOptionalToString(mRailHeadDistance) +
				(release == ReleaseVersion.IFC4X3_RC2 ? (mCantSegments.Count == 0 ? ",$" : ",(" + string.Join(",", mCantSegments.ConvertAll(x => "#" + x.Index)) + ")") : "");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			RailHeadDistance = ParserSTEP.StripDouble(str, ref pos, len);
			if(release <= ReleaseVersion.IFC4X3_RC3 && pos < len)
				mCantSegments.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcAlignmentCantSegment));
		}
	}
	public partial class IfcAlignmentCantSegment : IfcAlignmentParameterSegment
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mStartDistAlong) + "," +
			ParserSTEP.DoubleOptionalToString(mHorizontalLength) + "," + ParserSTEP.DoubleToString(mStartCantLeft) + "," +
			ParserSTEP.DoubleOptionalToString(mEndCantLeft) + "," + ParserSTEP.DoubleToString(mStartCantRight) + "," +
			ParserSTEP.DoubleOptionalToString(mEndCantRight) + "," + ParserSTEP.DoubleOptionalToString(mSmoothingLength) + ",." +
			mPredefinedType.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			StartDistAlong = ParserSTEP.StripDouble(str, ref pos, len);
			HorizontalLength = ParserSTEP.StripDouble(str, ref pos, len);
			StartCantLeft = ParserSTEP.StripDouble(str, ref pos, len);
			EndCantLeft = ParserSTEP.StripDouble(str, ref pos, len);
			StartCantRight = ParserSTEP.StripDouble(str, ref pos, len);
			EndCantRight = ParserSTEP.StripDouble(str, ref pos, len);
			SmoothingLength = ParserSTEP.StripDouble(str, ref pos, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAlignmentCantSegmentTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAlignmentCurve : IfcBoundedCurve //IFC4.1
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + "," + ParserSTEP.ObjToLinkString(mHorizontal) + "," + ParserSTEP.ObjToLinkString(mVertical) + "," + (mTag == "$" ? "$" : "'" + mTag + "'");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			mHorizontal = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcAlignment2DHorizontal;
			Vertical = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcAlignment2DVertical;
			mTag = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcAlignmentHorizontal : IfcLinearElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleOptionalToString(mStartDistAlong) +
				(release == ReleaseVersion.IFC4X3_RC2 ? (mHorizontalSegments.Count == 0 ? ",$" : ",(#" + string.Join(",#", mHorizontalSegments.ConvertAll(x => x.Index)) + ")") : ""); 
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			StartDistAlong = ParserSTEP.StripDouble(str, ref pos, len);
			if(pos < len && release < ReleaseVersion.IFC4X3_RC3)
				mHorizontalSegments.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcAlignmentHorizontalSegment));
		}
	}
	public partial class IfcAlignmentHorizontalSegment : IfcAlignmentParameterSegment
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + ",#" + mStartPoint.mIndex + "," + ParserSTEP.DoubleToString(mStartDirection) + "," +
			formatLength(mStartRadiusOfCurvature) + "," +
			formatLength(mEndRadiusOfCurvature) + "," + formatLength(mSegmentLength) + "," +
			ParserSTEP.DoubleOptionalToString(mGravityCenterLineHeight) + ",." + mPredefinedType.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mStartPoint = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCartesianPoint;
			mStartDirection = ParserSTEP.StripDouble(str, ref pos, len);
			StartRadiusOfCurvature = ParserSTEP.StripDouble(str, ref pos, len);
			EndRadiusOfCurvature = ParserSTEP.StripDouble(str, ref pos, len);
			SegmentLength = ParserSTEP.StripDouble(str, ref pos, len);
			GravityCenterLineHeight = ParserSTEP.StripDouble(str, ref pos, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAlignmentHorizontalSegmentTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public abstract partial class IfcAlignmentParameterSegment : BaseClassIfc
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) +
			(string.IsNullOrEmpty(mStartTag) ? ",$" : ",'" + ParserIfc.Encode(mStartTag) + "'") +
			(string.IsNullOrEmpty(mEndTag) ? ",$" : ",'" + ParserIfc.Encode(mEndTag) + "'");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			StartTag = ParserIfc.Decode(ParserSTEP.StripString(str, ref pos, len));
			EndTag = ParserIfc.Decode(ParserSTEP.StripString(str, ref pos, len));
		}
	}
	public partial class IfcAlignmentSegment : IfcLinearElement
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + ",#" + mDesignParameters.StepId; 
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			DesignParameters = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcAlignmentParameterSegment;
		}
	}
	public partial class IfcAlignmentVertical : IfcLinearElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + (release == ReleaseVersion.IFC4X3_RC2 ? (mVerticalSegments.Count == 0 ? ",$" : ",(#" + string.Join(",#", mVerticalSegments.ConvertAll(x => x.Index)) + ")") : "");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if(pos < len && release <= ReleaseVersion.IFC4X3_RC3)
				mVerticalSegments.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcAlignmentVerticalSegment));
		}
	}
	public partial class IfcAlignmentVerticalSegment : IfcAlignmentParameterSegment
	{
		protected override string BuildStringSTEP()
		{
			return base.BuildStringSTEP() + "," + ParserSTEP.DoubleOptionalToString(mStartDistAlong) + "," +
			ParserSTEP.DoubleOptionalToString(mHorizontalLength) + "," + ParserSTEP.DoubleOptionalToString(mStartHeight) + "," +
			ParserSTEP.DoubleToString(mStartGradient) + "," + ParserSTEP.DoubleToString(mEndGradient) +
			"," + ParserSTEP.DoubleOptionalToString(mRadiusOfCurvature) + ",." + mPredefinedType.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			StartDistAlong = ParserSTEP.StripDouble(str, ref pos, len);
			HorizontalLength = ParserSTEP.StripDouble(str, ref pos, len);
			StartHeight = ParserSTEP.StripDouble(str, ref pos, len);
			StartGradient = ParserSTEP.StripDouble(str, ref pos, len);
			EndGradient = ParserSTEP.StripDouble(str, ref pos, len);
			RadiusOfCurvature = ParserSTEP.StripDouble(str, ref pos, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAlignmentVerticalSegmentTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAnnotation : IfcProduct
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4X3_RC1 ? "" : (mPredefinedType == IfcAnnotationTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release > ReleaseVersion.IFC4X2)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcAnnotationTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcAnnotationFillArea : IfcGeometricRepresentationItem
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + ",#" + mOuterBoundary.Index + (mInnerBoundaries.Count == 0 ? ",$" : ",(#" + string.Join(",#", mInnerBoundaries.ConvertAll(x=>x.mIndex)) + ")");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mOuterBoundary = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCurve;
			mInnerBoundaries.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x=> dictionary[x] as IfcCurve));
		}
	}
	public partial class IfcAnnotationFillAreaOccurrence : IfcAnnotationOccurrence //IFC4 DEPRECATED
	{
		protected override string BuildStringSTEP(ReleaseVersion release) 
		{ 
			return base.BuildStringSTEP(release) + (mFillStyleTarget == null ? ",$" : ",#" + mFillStyleTarget) +
				(mGlobalOrLocal == null ? ",$" : ",." + mGlobalOrLocal.Value.ToString() + "."); 
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mFillStyleTarget = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcPoint;
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s != "$")
			{
				IfcGlobalOrLocalEnum val = IfcGlobalOrLocalEnum.GLOBAL_COORDS;
				if (Enum.TryParse<IfcGlobalOrLocalEnum>(s.Replace(".", ""), true, out val))
					mGlobalOrLocal = val;
			}
		}
	}
	public partial class IfcAnnotationSurface : IfcGeometricRepresentationItem //DEPRECATED IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",#" + mItem + "," + ParserSTEP.ObjToLinkString(mTextureCoordinates); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mItem = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcGeometricRepresentationItem;
			mTextureCoordinates = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcTextureCoordinate;
		}
	}
	public partial class IfcApplication : BaseClassIfc
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + (mApplicationDeveloper != null ? ",#" + mApplicationDeveloper.Index : ",$") + ",'" + mVersion + "','" +
				(string.IsNullOrEmpty(mApplicationFullName) ? mDatabase.Factory.ApplicationFullName : mApplicationFullName) + "','" +
				(string.IsNullOrEmpty(mApplicationIdentifier) ? mDatabase.Factory.ApplicationIdentifier : mApplicationIdentifier) + "'";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			ApplicationDeveloper = dictionary[ParserSTEP.StripLink(str,ref pos, len)] as IfcOrganization;
			mVersion = ParserSTEP.StripString(str, ref pos, len);
			mApplicationFullName = ParserSTEP.StripString(str, ref pos, len);
			mApplicationIdentifier = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcAppliedValue : BaseClassIfc, IfcMetricValueSelect, IfcObjectReferenceSelect, IfcResourceObjectSelect// SUPERTYPE OF(IfcCostValue);
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			string result = base.BuildStringSTEP(release) + (mName == "$" ? ",$," : ",'" + mName + "',") + (mDescription == "$" ? "$," : "'" + mDescription + "',");
			if (mAppliedValue == null)
				result += "$";
			else
			{
				IfcValue val = mAppliedValue as IfcValue;
				if (val != null)
					result += val.ToString();
				else
					result += ParserSTEP.LinkToString((mAppliedValue as BaseClassIfc).StepId);
			}
			result += "," + ParserSTEP.LinkToString(mUnitBasis) + ",";
			if (release < ReleaseVersion.IFC4)
				return result +  (mSSApplicableDate == null ? ",$" : ",#" + mSSApplicableDate.Index) + (mSSFixedUntilDate == null ? ",$" : ",#" + mSSFixedUntilDate.Index);
			result += IfcDate.STEPAttribute(mApplicableDate) + "," + IfcDate.STEPAttribute(mFixedUntilDate);
			result += (mCategory == "$" ? ",$," : ",'" + mCategory + "',") + (mCondition == "$" ? "$," : "'" + mCondition + "',");
			return result + (mArithmeticOperator == IfcArithmeticOperatorEnum.NONE ? "$," : "." + mArithmeticOperator.ToString() + ".,")
				+ (mComponents.Count == 0 ? "$" : "(#" + string.Join(",#", mComponents.Select(x => x.StepId)) + ")"); ;
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mName = ParserSTEP.StripString(str, ref pos, len);
			mDescription = ParserSTEP.StripString(str, ref pos, len);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if(s.StartsWith("IFC"))
				mAppliedValue = ParserIfc.parseValue(s);
			else
				mAppliedValue = dictionary[ ParserSTEP.ParseLink(s)] as IfcAppliedValueSelect;
			mUnitBasis = ParserSTEP.StripLink(str, ref pos, len);
			if(release < ReleaseVersion.IFC4)
			{
				mSSApplicableDate = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDateTimeSelect;
				mSSFixedUntilDate = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDateTimeSelect;
			}
			else
			{
				mApplicableDate = IfcDate.ParseSTEP(ParserSTEP.StripString(str, ref pos, len));
				mFixedUntilDate = IfcDate.ParseSTEP(ParserSTEP.StripString(str, ref pos, len));
				mCategory = ParserSTEP.StripString(str, ref pos, len);
				mCondition = ParserSTEP.StripString(str, ref pos, len);
				s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcArithmeticOperatorEnum>(s.Replace(".", ""), true, out mArithmeticOperator);
				Components.AddRange(ParserSTEP.StripListLink(str, ref pos, len).Select(x => dictionary[x] as IfcAppliedValue));
			}
		}
	}
	public partial class IfcAppliedValueRelationship : BaseClassIfc //DEPRECATED IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + ",#" + mComponentOfTotal.StepId + ",(#" + 
				string.Join(",#", mComponents.Select(x=>x.Index))+ "),." + mArithmeticOperator.ToString() + ".," + mName + "," + mDescription;
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			ComponentOfTotal = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcAppliedValue;
			Components.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x=>dictionary[x] as IfcAppliedValue));
			Enum.TryParse<IfcArithmeticOperatorEnum>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), true, out mArithmeticOperator);
			mName = ParserSTEP.StripString(str, ref pos, len);
			mDescription = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcApproval : BaseClassIfc, IfcResourceObjectSelect
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release)  + "," + mDescription + "," + ParserSTEP.LinkToString(mApprovalDateTime) + "," +  mApprovalStatus + "," + mApprovalLevel + "," + mApprovalQualifier + "," + mName + "," + mIdentifier;  }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mDescription = ParserSTEP.StripString(str, ref pos, len);
			mApprovalDateTime = ParserSTEP.StripLink(str, ref pos, len);
			mApprovalStatus = ParserSTEP.StripString(str, ref pos, len);
			mApprovalLevel = ParserSTEP.StripString(str, ref pos, len);
			mApprovalQualifier = ParserSTEP.StripString(str, ref pos, len);
			mName = ParserSTEP.StripString(str, ref pos, len);
			mIdentifier = ParserSTEP.StripString(str, ref pos, len);
		}
	}
	public partial class IfcApprovalActorRelationship : BaseClassIfc //DEPRECATED IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.LinkToString(mActor) + "," + ParserSTEP.LinkToString(mApproval) + "," + ParserSTEP.LinkToString(mRole);  }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mActor = ParserSTEP.StripLink(str, ref pos, len);
			mApproval = ParserSTEP.StripLink(str, ref pos, len);
			mRole = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	public partial class IfcApprovalPropertyRelationship : BaseClassIfc //DEPRECATED IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			string str = base.BuildStringSTEP(release) + ",(" + ParserSTEP.LinkToString(mApprovedProperties[0]);
			for(int icounter = 1; icounter < mApprovedProperties.Count; icounter++)
				str += "," + ParserSTEP.LinkToString(mApprovedProperties[icounter]);
			str += ")," + ParserSTEP.LinkToString(mApproval);
			return str;
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mApprovedProperties = ParserSTEP.StripListLink(str, ref pos, len);
			mApproval = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	public partial class IfcApprovalRelationship : IfcResourceLevelRelationship //IFC4Change
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.LinkToString(mRelatedApproval) + "," + ParserSTEP.LinkToString(mRelatingApproval) + (release < ReleaseVersion.IFC4 ? (mDescription == "$" ? ",$,'" : ",'" + mDescription + "','") +  mName  + "'": ""); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mRelatedApproval = ParserSTEP.StripLink(str, ref pos, len); 
			mRelatingApproval = ParserSTEP.StripLink(str, ref pos, len);
			if (release < ReleaseVersion.IFC4)
			{
				mDescription = ParserSTEP.StripString(str, ref pos, len);
				mName = ParserSTEP.StripString(str, ref pos, len);
			}
		}
	}
	public partial class IfcArbitraryClosedProfileDef : IfcProfileDef //SUPERTYPE OF(IfcArbitraryProfileDefWithVoids)
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.LinkToString(mOuterCurve); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mOuterCurve = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	public partial class IfcArbitraryOpenProfileDef : IfcProfileDef //	SUPERTYPE OF(IfcCenterLineProfileDef)
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.LinkToString(mCurve); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mCurve = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	public partial class IfcArbitraryProfileDefWithVoids : IfcArbitraryClosedProfileDef
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			if (mInnerCurves.Count == 0)
				return base.BuildStringSTEP(release);
			string str = base.BuildStringSTEP(release) + ",(" + ParserSTEP.LinkToString(mInnerCurves[0]);
			for (int icounter = 1; icounter < mInnerCurves.Count; icounter++)
				str += "," + ParserSTEP.LinkToString(mInnerCurves[icounter]);
			return str + ")";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mInnerCurves = ParserSTEP.StripListLink(str, ref pos, len);
		}
	}
	public partial class IfcAsset : IfcGroup
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + (string.IsNullOrEmpty(mIdentification) ? ",$," : ",'" + ParserIfc.Encode(mIdentification) + "',") + ParserSTEP.LinkToString(mOriginalValue) + "," +ParserSTEP.LinkToString(mCurrentValue) + "," + 
				ParserSTEP.LinkToString(mTotalReplacementCost) + "," +ParserSTEP.LinkToString(mOwner) + "," +
				ParserSTEP.LinkToString(mUser) + "," +ParserSTEP.LinkToString(mResponsiblePerson) + "," +
				(mDatabase.Release < ReleaseVersion.IFC4 ? ParserSTEP.LinkToString(mIncorporationDateSS) : IfcDate.STEPAttribute(mIncorporationDate)) + "," +ParserSTEP.LinkToString(mDepreciatedValue);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mIdentification = ParserIfc.Decode(ParserSTEP.StripString(str, ref pos, len));
			mOriginalValue = ParserSTEP.StripLink(str, ref pos, len);
			mCurrentValue = ParserSTEP.StripLink(str, ref pos, len);
			mTotalReplacementCost = ParserSTEP.StripLink(str, ref pos, len);
			mOwner = ParserSTEP.StripLink(str, ref pos, len);
			mUser = ParserSTEP.StripLink(str, ref pos, len);
			mResponsiblePerson = ParserSTEP.StripLink(str, ref pos, len);
			if (release < ReleaseVersion.IFC4)
				mIncorporationDateSS = ParserSTEP.StripLink(str, ref pos, len);
			else
				mIncorporationDate = IfcDate.ParseSTEP(ParserSTEP.StripString(str, ref pos, len));
			mDepreciatedValue = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	public partial class IfcAsymmetricIShapeProfileDef : IfcParameterizedProfileDef // Ifc2x3 IfcIShapeProfileDef 
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			if (mDatabase.Release < ReleaseVersion.IFC4)
			{
				return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleToString(mBottomFlangeWidth) + "," + ParserSTEP.DoubleToString(mOverallDepth) + "," + 
					ParserSTEP.DoubleToString(mWebThickness) + "," + ParserSTEP.DoubleToString(mBottomFlangeThickness) + "," + ParserSTEP.DoubleOptionalToString(mBottomFlangeFilletRadius) + "," +
					ParserSTEP.DoubleToString(mTopFlangeWidth) + "," + ParserSTEP.DoubleOptionalToString(mTopFlangeThickness) + "," +
					ParserSTEP.DoubleOptionalToString(mTopFlangeFilletRadius) +  "," + ParserSTEP.DoubleOptionalToString(mCentreOfGravityInY);
			}
			return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleToString(mBottomFlangeWidth) + "," + ParserSTEP.DoubleToString(mOverallDepth) + "," +
					ParserSTEP.DoubleToString(mWebThickness) + "," + ParserSTEP.DoubleToString(mBottomFlangeThickness) + "," + ParserSTEP.DoubleOptionalToString(mBottomFlangeFilletRadius) + "," +
				ParserSTEP.DoubleToString(mTopFlangeWidth) + "," + ParserSTEP.DoubleOptionalToString(mTopFlangeThickness) + "," +
				ParserSTEP.DoubleOptionalToString(mTopFlangeFilletRadius) + "," + ParserSTEP.DoubleOptionalToString(mBottomFlangeEdgeRadius) + "," +
				ParserSTEP.DoubleOptionalToString(mBottomFlangeSlope) + "," + ParserSTEP.DoubleOptionalToString(mTopFlangeEdgeRadius) + "," +
				ParserSTEP.DoubleOptionalToString(mTopFlangeSlope);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release < ReleaseVersion.IFC4)
			{
				mBottomFlangeWidth = ParserSTEP.StripDouble(str, ref pos, len);
				mOverallDepth = ParserSTEP.StripDouble(str, ref pos, len);
				mWebThickness = ParserSTEP.StripDouble(str, ref pos, len);
				mBottomFlangeThickness = ParserSTEP.StripDouble(str, ref pos, len);
				mBottomFlangeFilletRadius = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeWidth = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeThickness = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeFilletRadius = ParserSTEP.StripDouble(str, ref pos, len);
				mCentreOfGravityInY = ParserSTEP.StripDouble(str, ref pos, len);
			}
			else
			{
				mBottomFlangeWidth = ParserSTEP.StripDouble(str, ref pos, len);
				mOverallDepth = ParserSTEP.StripDouble(str, ref pos, len);
				mWebThickness = ParserSTEP.StripDouble(str, ref pos, len);
				mBottomFlangeThickness = ParserSTEP.StripDouble(str, ref pos, len);
				mBottomFlangeFilletRadius = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeWidth = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeThickness = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeFilletRadius = ParserSTEP.StripDouble(str, ref pos, len);
				mBottomFlangeEdgeRadius = ParserSTEP.StripDouble(str, ref pos, len);
				mBottomFlangeSlope = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeEdgeRadius = ParserSTEP.StripDouble(str, ref pos, len);
				mTopFlangeSlope = ParserSTEP.StripDouble(str, ref pos, len);
			}
		}
	}
	public partial class IfcAudioVisualAppliance : IfcFlowTerminal //IFC4
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : ( mPredefinedType == IfcAudioVisualApplianceTypeEnum.NOTDEFINED  ? ",$" : ",." + mPredefinedType.ToString() + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcAudioVisualApplianceTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcAudioVisualApplianceType : IfcFlowTerminalType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcAudioVisualApplianceTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcAxis1Placement : IfcPlacement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.LinkToString(mAxis); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mAxis = ParserSTEP.StripLink(str, ref pos, len);
		}
	}
	public partial class IfcAxis2Placement2D : IfcPlacement, IfcAxis2Placement
	{ 
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.ObjToLinkString(mRefDirection); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mRefDirection = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDirection;
		}
	}
	public partial class IfcAxis2Placement3D : IfcPlacement, IfcAxis2Placement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.ObjToLinkString(mAxis) + "," + ParserSTEP.ObjToLinkString(mRefDirection); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mAxis = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDirection;
			mRefDirection = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDirection;
		}
	}
	public partial class IfcAxis2PlacementLinear : IfcPlacement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.ObjToLinkString(mAxis) + "," + ParserSTEP.ObjToLinkString(mRefDirection); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mAxis = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDirection;
			mRefDirection = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDirection;
		}
	}
}
