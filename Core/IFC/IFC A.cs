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
using System.Collections.Specialized;
using System.Text;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using System.Linq;
using GeometryGym.STEP;

namespace GeometryGym.Ifc
{  
	[Serializable]
	public partial class IfcActionRequest : IfcControl
	{
		//internal string mRequestID;// : IfcIdentifier; IFC4 relocated
		internal IfcActionRequest() : base() { }
		internal IfcActionRequest(DatabaseIfc db, IfcActionRequest r, DuplicateOptions options) : base(db,r, options) { }
	}
	[Serializable]
	public partial class IfcActor : IfcObject // SUPERTYPE OF(IfcOccupant)
	{
		internal IfcActorSelect mTheActor = null;//	 :	IfcActorSelect; 
		//INVERSE
		internal SET<IfcRelAssignsToActor> mIsActingUpon = new SET<IfcRelAssignsToActor>();// : SET [0:?] OF IfcRelAssignsToActor FOR RelatingActor;

		public IfcActorSelect TheActor { get { return mTheActor; } set { mTheActor = value; } }
		public SET<IfcRelAssignsToActor> IsActingUpon { get { return mIsActingUpon; } }

		internal IfcActor() : base() { }
		internal IfcActor(DatabaseIfc db, IfcActor actor, DuplicateOptions options) : base(db, actor, options) { TheActor = db.Factory.Duplicate(actor.TheActor as BaseClassIfc) as IfcActorSelect; }
		public IfcActor(IfcActorSelect actor) : base(actor.Database) { mTheActor = actor; }
	}
	[Serializable]
	public partial class IfcActorRole : BaseClassIfc, IfcResourceObjectSelect
	{
		internal IfcRoleEnum mRole = IfcRoleEnum.NOTDEFINED;// : OPTIONAL IfcRoleEnum
		internal string mUserDefinedRole = "$";// : OPTIONAL IfcLabel
		internal string mDescription = "$";// : OPTIONAL IfcText; 
										   //INVERSE
		private SET<IfcExternalReferenceRelationship> mHasExternalReference = new SET<IfcExternalReferenceRelationship>(); //IFC4 SET [0:?] OF IfcExternalReferenceRelationship FOR RelatedResourceObjects;
		internal List<IfcResourceConstraintRelationship> mHasConstraintRelationships = new List<IfcResourceConstraintRelationship>(); //gg

		public IfcRoleEnum Role { get { return mRole; } set { mRole = value; } }
		public string UserDefinedRole { get { return (mUserDefinedRole == "$" ? "" : ParserIfc.Decode(mUserDefinedRole)); } set { mUserDefinedRole = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		public string Description { get { return (mDescription == "$" ? "" : ParserIfc.Decode(mDescription)); } set { mDescription = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		public SET<IfcExternalReferenceRelationship> HasExternalReference { get { return mHasExternalReference; } set { mHasExternalReference.Clear();  if (value != null) { mHasExternalReference.CollectionChanged -= mHasExternalReference_CollectionChanged; mHasExternalReference = value; mHasExternalReference.CollectionChanged += mHasExternalReference_CollectionChanged; } } }
		public ReadOnlyCollection<IfcResourceConstraintRelationship> HasConstraintRelationships { get { return new ReadOnlyCollection<IfcResourceConstraintRelationship>(mHasConstraintRelationships); } }

		internal IfcActorRole() : base() { }
		internal IfcActorRole(DatabaseIfc db, IfcActorRole r) : base(db,r) { mRole = r.mRole; mDescription = r.mDescription; mUserDefinedRole = r.mUserDefinedRole; }
		public IfcActorRole(DatabaseIfc db) : base(db) { }

		protected override void initialize()
		{
			base.initialize();

			mHasExternalReference.CollectionChanged += mHasExternalReference_CollectionChanged;
		}
		private void mHasExternalReference_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (mDatabase != null && mDatabase.IsDisposed())
				return;
			if (e.NewItems != null)
			{
				foreach (IfcExternalReferenceRelationship r in e.NewItems)
				{
					if (!r.RelatedResourceObjects.Contains(this))
						r.RelatedResourceObjects.Add(this);
				}
			}
			if (e.OldItems != null)
			{
				foreach (IfcExternalReferenceRelationship r in e.OldItems)
					r.RelatedResourceObjects.Remove(this);
			}
		}

		public void AddConstraintRelationShip(IfcResourceConstraintRelationship constraintRelationship) { mHasConstraintRelationships.Add(constraintRelationship); }
	}
	public interface IfcActorSelect : IBaseClassIfc {  }// IfcOrganization,  IfcPerson,  IfcPersonAndOrganization);
	[Serializable]
	public partial class IfcActuator : IfcDistributionControlElement //IFC4  
	{   
		internal IfcActuatorTypeEnum mPredefinedType = IfcActuatorTypeEnum.NOTDEFINED;
		public IfcActuatorTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcActuator() : base() { }
		internal IfcActuator(DatabaseIfc db, IfcActuator a, DuplicateOptions options) : base(db,a, options) { mPredefinedType = a.mPredefinedType; }
		public IfcActuator(IfcObjectDefinition host, IfcObjectPlacement placement, IfcProductDefinitionShape representation, IfcDistributionSystem system) : base(host, placement,representation, system) { }
	}
	[Serializable]
	public partial class IfcActuatorType : IfcDistributionControlElementType
	{
		internal IfcActuatorTypeEnum mPredefinedType = IfcActuatorTypeEnum.NOTDEFINED;// : IfcActuatorTypeEnum; 
		public IfcActuatorTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcActuatorType() : base() { }
		internal IfcActuatorType(DatabaseIfc db, IfcActuatorType t, DuplicateOptions options) : base(db, t, options)  { mPredefinedType = t.mPredefinedType; }
		public IfcActuatorType(DatabaseIfc m, string name, IfcActuatorTypeEnum t) : base(m) { Name = name; mPredefinedType = t; }
	}
	[Serializable]
	public abstract partial class IfcAddress : BaseClassIfc, IfcObjectReferenceSelect   //ABSTRACT SUPERTYPE OF(ONEOF(IfcPostalAddress, IfcTelecomAddress));
	{
		internal IfcAddressTypeEnum mPurpose = IfcAddressTypeEnum.NOTDEFINED;// : OPTIONAL IfcAddressTypeEnum
		internal string mDescription = "$";// : OPTIONAL IfcText;
		internal string mUserDefinedPurpose = "$";// : OPTIONAL IfcLabel 

		public IfcAddressTypeEnum Purpose { get { return mPurpose; } set { mPurpose = value; } }
		public string Description { get { return mDescription == "$" ? "" : ParserIfc.Decode(mDescription); } set { mDescription = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		public string UserDefinedPurpose { get { return mUserDefinedPurpose == "$" ? "" : ParserIfc.Decode(mUserDefinedPurpose); } set { mUserDefinedPurpose = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		
		protected IfcAddress() : base() { }
		protected IfcAddress(DatabaseIfc db) : base(db) {  }
		protected IfcAddress(DatabaseIfc db, IfcAddress a) : base(db,a)
		{
			mPurpose = a.mPurpose;
			mDescription = a.mDescription;
			mUserDefinedPurpose = a.mUserDefinedPurpose;
		}
	}
	[Serializable]
	public partial class IfcAdvancedBrep : IfcManifoldSolidBrep // IFC4
	{
		internal IfcAdvancedBrep() : base() { }
		public IfcAdvancedBrep(List<IfcAdvancedFace> faces) : base(new IfcClosedShell(faces.ConvertAll(x => (IfcFace)x))) { }
		internal IfcAdvancedBrep(DatabaseIfc db, IfcAdvancedBrep b, DuplicateOptions options) : base(db, b, options) { }
		public IfcAdvancedBrep(IfcClosedShell s) : base(s) { }
	}
	[Serializable]
	public partial class IfcAdvancedBrepWithVoids : IfcAdvancedBrep
	{
		private SET<IfcClosedShell> mVoids = new SET<IfcClosedShell>();// : SET [1:?] OF IfcClosedShell
		public SET<IfcClosedShell> Voids { get { return mVoids; } }

		internal IfcAdvancedBrepWithVoids() : base() { }
		internal IfcAdvancedBrepWithVoids(DatabaseIfc db, IfcAdvancedBrepWithVoids b, DuplicateOptions options) : base(db, b, options) { mVoids.AddRange(b.Voids.ConvertAll(x=> db.Factory.Duplicate(x) as IfcClosedShell)); }
		public IfcAdvancedBrepWithVoids(IfcClosedShell s, IEnumerable<IfcClosedShell> voids) : base(s) { Voids.AddRange(voids); }
	}
	[Serializable]
	public partial class IfcAdvancedFace : IfcFaceSurface
	{
		internal IfcAdvancedFace() : base() { }
		internal IfcAdvancedFace(DatabaseIfc db, IfcAdvancedFace f, DuplicateOptions options) : base(db, f, options) { }
		public IfcAdvancedFace(IfcFaceOuterBound bound, IfcSurface f, bool sense) : base(bound, f, sense) { }
		public IfcAdvancedFace(List<IfcFaceBound> bounds, IfcSurface f, bool sense) : base(bounds, f, sense) { }
		public IfcAdvancedFace(IfcFaceOuterBound outer, IfcFaceBound inner, IfcSurface f, bool sense) : base(outer,inner, f, sense) { }
	}
	[Serializable]
	public partial class IfcAirTerminal : IfcFlowTerminal //IFC4
	{
		internal IfcAirTerminalTypeEnum mPredefinedType = IfcAirTerminalTypeEnum.NOTDEFINED;// OPTIONAL : IfcAirTerminalTypeEnum;
		public IfcAirTerminalTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAirTerminal() : base() { }
		internal IfcAirTerminal(DatabaseIfc db, IfcAirTerminal t, DuplicateOptions options) : base(db, t, options) { mPredefinedType = t.mPredefinedType; }
		public IfcAirTerminal(IfcObjectDefinition host, IfcObjectPlacement placement, IfcProductDefinitionShape representation, IfcDistributionSystem system) : base(host, placement, representation, system) { }
	}
	[Serializable]
	public partial class IfcAirTerminalBox : IfcFlowController //IFC4
	{
		internal IfcAirTerminalBoxTypeEnum mPredefinedType = IfcAirTerminalBoxTypeEnum.NOTDEFINED;// OPTIONAL : IfcAirTerminalBoxTypeEnum;
		public IfcAirTerminalBoxTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAirTerminalBox() : base() { }
		internal IfcAirTerminalBox(DatabaseIfc db, IfcAirTerminalBox b, DuplicateOptions options) : base(db, b, options) { mPredefinedType = b.mPredefinedType; }
		public IfcAirTerminalBox(IfcObjectDefinition host, IfcObjectPlacement placement, IfcProductDefinitionShape representation, IfcDistributionSystem system) : base(host, placement, representation, system) { }
	}
	[Serializable]
	public partial class IfcAirTerminalBoxType : IfcFlowControllerType
	{
		internal IfcAirTerminalBoxTypeEnum mPredefinedType = IfcAirTerminalBoxTypeEnum.NOTDEFINED;// : IfcAirTerminalBoxTypeEnum; 
		public IfcAirTerminalBoxTypeEnum PredefinedType { get { return mPredefinedType;} set { mPredefinedType = value; } }

		internal IfcAirTerminalBoxType() : base() { }
		internal IfcAirTerminalBoxType(DatabaseIfc db, IfcAirTerminalBoxType t, DuplicateOptions options) : base(db, t, options) { mPredefinedType = t.mPredefinedType; }
		public IfcAirTerminalBoxType(DatabaseIfc m, string name, IfcAirTerminalBoxTypeEnum type) : base(m) { Name = name; mPredefinedType = type; }
	}
	[Serializable]
	public partial class IfcAirTerminalType : IfcFlowTerminalType
	{
		internal IfcAirTerminalTypeEnum mPredefinedType = IfcAirTerminalTypeEnum.NOTDEFINED;// : IfcAirTerminalBoxTypeEnum; 
		public IfcAirTerminalTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAirTerminalType() : base() { }
		internal IfcAirTerminalType(DatabaseIfc db, IfcAirTerminalType t, DuplicateOptions options) : base(db, t, options) { mPredefinedType = t.mPredefinedType; }
		public IfcAirTerminalType(DatabaseIfc m, string name, IfcAirTerminalTypeEnum type) : base(m) { Name = name; mPredefinedType = type; }
	}
	[Serializable]
	public partial class IfcAirToAirHeatRecovery : IfcEnergyConversionDevice //IFC4  
	{
		internal IfcAirToAirHeatRecoveryTypeEnum mPredefinedType = IfcAirToAirHeatRecoveryTypeEnum.NOTDEFINED;
		public IfcAirToAirHeatRecoveryTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAirToAirHeatRecovery() : base() { }
		internal IfcAirToAirHeatRecovery(DatabaseIfc db, IfcAirToAirHeatRecovery a, DuplicateOptions options) : base(db, a, options) { mPredefinedType = a.mPredefinedType; }
		public IfcAirToAirHeatRecovery(IfcObjectDefinition host, IfcObjectPlacement placement, IfcProductDefinitionShape representation, IfcDistributionSystem system) : base(host, placement, representation, system) { }
	}
	[Serializable]
	public partial class IfcAirToAirHeatRecoveryType : IfcEnergyConversionDeviceType
	{
		internal IfcAirToAirHeatRecoveryTypeEnum mPredefinedType = IfcAirToAirHeatRecoveryTypeEnum.NOTDEFINED;// : IfcAirToAirHeatRecoveryTypeEnum; 
		public IfcAirToAirHeatRecoveryTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAirToAirHeatRecoveryType() : base() { }
		internal IfcAirToAirHeatRecoveryType(DatabaseIfc db, IfcAirToAirHeatRecoveryType t, DuplicateOptions options) : base(db, t, options) { mPredefinedType = t.mPredefinedType; }
		public IfcAirToAirHeatRecoveryType(DatabaseIfc m, string name, IfcAirToAirHeatRecoveryTypeEnum type) : base(m) { Name = name; mPredefinedType = type; }
	}
	[Serializable]
	public partial class IfcAlarm : IfcDistributionControlElement //IFC4  
	{
		internal IfcAlarmTypeEnum mPredefinedType = IfcAlarmTypeEnum.NOTDEFINED;
		public IfcAlarmTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAlarm() : base() { }
		internal IfcAlarm(DatabaseIfc db, IfcAlarm a, DuplicateOptions options) : base(db, a, options) { mPredefinedType = a.mPredefinedType; }
		public IfcAlarm(IfcObjectDefinition host, IfcObjectPlacement placement, IfcProductDefinitionShape representation, IfcDistributionSystem system) : base(host, placement, representation, system) { }
	}
	[Serializable]
	public partial class IfcAlarmType : IfcDistributionControlElementType
	{
		internal IfcAlarmTypeEnum mPredefinedType = IfcAlarmTypeEnum.NOTDEFINED;// : IfcAlarmTypeEnum; 
		public IfcAlarmTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAlarmType() : base() { }
		internal IfcAlarmType(DatabaseIfc db, IfcAlarmType t, DuplicateOptions options) : base(db, t, options) { mPredefinedType = t.mPredefinedType; }
		public IfcAlarmType(DatabaseIfc m, string name, IfcAlarmTypeEnum t) : base(m) { Name = name; mPredefinedType = t; }
	}
	[Serializable]
	public partial class IfcAlignment : IfcLinearPositioningElement //IFC4.1
	{
		internal IfcAlignmentTypeEnum mPredefinedType = IfcAlignmentTypeEnum.NOTDEFINED;// : OPTIONAL IfcAlignmentTypeEnum;
		public IfcAlignmentTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAlignment() : base() { }
		internal IfcAlignment(DatabaseIfc db, IfcAlignment alignment, DuplicateOptions options) 
			: base(db, alignment, options) { PredefinedType = alignment.PredefinedType; }
		public IfcAlignment(IfcSite host, IfcCurve axis) : base(host, axis) { }
	}
	[Serializable]
	public partial class IfcAlignment2DCant : IfcAxisLateralInclination
	{
		private LIST<IfcAlignment2DCantSegment> mSegments = new LIST<IfcAlignment2DCantSegment>(); //: LIST[1:?] OF IfcAlignment2DCantSegment;
		private double mRailHeadDistance = 0; //: IfcPositiveLengthMeasure;

		public LIST<IfcAlignment2DCantSegment> Segments { get { return mSegments; } set { mSegments = value; } }
		public double RailHeadDistance { get { return mRailHeadDistance; } set { mRailHeadDistance = value; } }

		public IfcAlignment2DCant() : base() { }
		internal IfcAlignment2DCant(DatabaseIfc db, IfcAlignment2DCant alignment2dCant, DuplicateOptions options) : base(db, alignment2dCant, options)
		{
			Segments.AddRange(alignment2dCant.Segments.Select(x => db.Factory.Duplicate(x) as IfcAlignment2DCantSegment));
			RailHeadDistance = alignment2dCant.RailHeadDistance;
		}
		public IfcAlignment2DCant(DatabaseIfc db, IEnumerable<IfcAlignment2DCantSegment> segments, double railHeadDistance)
			: base(db)
		{
			Segments.AddRange(segments);
			RailHeadDistance = railHeadDistance;
		}
	}
	[Serializable]
	public partial class IfcAlignment2DCantSegLine : IfcAlignment2DCantSegment
	{
		public IfcAlignment2DCantSegLine() : base() { }
		internal IfcAlignment2DCantSegLine(DatabaseIfc db, IfcAlignment2DCantSegLine alignment2DCantSegLine, DuplicateOptions options) : base(db, alignment2DCantSegLine, options) { }
		public IfcAlignment2DCantSegLine(DatabaseIfc db, double startDistAlong, double horizontalLength, double startCantLeft, double startCantRight)
			: base(db, startDistAlong, horizontalLength, startCantLeft, startCantRight) { }
	}
	[Serializable]
	public abstract partial class IfcAlignment2DCantSegment : IfcAlignment2DSegment
	{
		private double mStartDistAlong = 0; //: IfcPositiveLengthMeasure;
		private double mHorizontalLength = 0; //: IfcPositiveLengthMeasure;
		private double mStartCantLeft = 0; //: IfcLengthMeasure;
		private double mEndCantLeft = double.NaN; //: OPTIONAL IfcLengthMeasure;
		private double mStartCantRight = 0; //: IfcLengthMeasure;
		private double mEndCantRight = double.NaN; //: OPTIONAL IfcLengthMeasure;
												   //INVERSE
		private SET<IfcAlignment2DCant> mToCant = new SET<IfcAlignment2DCant>();

		public double StartDistAlong { get { return mStartDistAlong; } set { mStartDistAlong = value; } }
		public double HorizontalLength { get { return mHorizontalLength; } set { mHorizontalLength = value; } }
		public double StartCantLeft { get { return mStartCantLeft; } set { mStartCantLeft = value; } }
		public double EndCantLeft { get { return mEndCantLeft; } set { mEndCantLeft = value; } }
		public double StartCantRight { get { return mStartCantRight; } set { mStartCantRight = value; } }
		public double EndCantRight { get { return mEndCantRight; } set { mEndCantRight = value; } }
		//INVERSE
		public SET<IfcAlignment2DCant> ToCant { get { return mToCant; } set { mToCant = value; } }

		protected IfcAlignment2DCantSegment() : base() { }
		protected IfcAlignment2DCantSegment(DatabaseIfc db, IfcAlignment2DCantSegment alignment2DCantSegment, DuplicateOptions options) : base(db, alignment2DCantSegment, options)
		{
			StartDistAlong = alignment2DCantSegment.StartDistAlong;
			HorizontalLength = alignment2DCantSegment.HorizontalLength;
			StartCantLeft = alignment2DCantSegment.StartCantLeft;
			EndCantLeft = alignment2DCantSegment.EndCantLeft;
			StartCantLeft = alignment2DCantSegment.StartCantLeft;
			EndCantRight = alignment2DCantSegment.EndCantRight;
		}
		protected IfcAlignment2DCantSegment(DatabaseIfc db, double startDistAlong, double horizontalLength, double startCantLeft, double startCantRight)
			: base(db)
		{
			StartDistAlong = startDistAlong;
			HorizontalLength = horizontalLength;
			StartCantLeft = startCantLeft;
			StartCantRight = startCantRight;
		}
	}
	[Serializable]
	public partial class IfcAlignment2DCantSegTransition : IfcAlignment2DCantSegment
	{
		private double mStartRadius = double.NaN; //: OPTIONAL IfcPositiveLengthMeasure;
		private double mEndRadius = double.NaN; //: OPTIONAL IfcPositiveLengthMeasure;
		private bool mIsStartRadiusCCW = false; //: IfcBoolean;
		private bool mIsEndRadiusCCW = false; //: IfcBoolean;
		private IfcTransitionCurveType mTransitionCurveType = IfcTransitionCurveType.BIQUADRATICPARABOLA; //: IfcTransitionCurveType;

		public double StartRadius { get { return mStartRadius; } set { mStartRadius = value; } }
		public double EndRadius { get { return mEndRadius; } set { mEndRadius = value; } }
		public bool IsStartRadiusCCW { get { return mIsStartRadiusCCW; } set { mIsStartRadiusCCW = value; } }
		public bool IsEndRadiusCCW { get { return mIsEndRadiusCCW; } set { mIsEndRadiusCCW = value; } }
		public IfcTransitionCurveType TransitionCurveType { get { return mTransitionCurveType; } set { mTransitionCurveType = value; } }

		public IfcAlignment2DCantSegTransition() : base() { }
		internal IfcAlignment2DCantSegTransition(DatabaseIfc db, IfcAlignment2DCantSegTransition alignment2DCantSegTransition, DuplicateOptions options) : base(db, alignment2DCantSegTransition, options)
		{
			StartRadius = alignment2DCantSegTransition.StartRadius;
			EndRadius = alignment2DCantSegTransition.EndRadius;
			IsStartRadiusCCW = alignment2DCantSegTransition.IsStartRadiusCCW;
			IsEndRadiusCCW = alignment2DCantSegTransition.IsEndRadiusCCW;
			TransitionCurveType = alignment2DCantSegTransition.TransitionCurveType;
		}
		public IfcAlignment2DCantSegTransition(DatabaseIfc db, double startDistAlong, double horizontalLength, double startCantLeft, double startCantRight, bool isStartRadiusCCW, bool isEndRadiusCCW, IfcTransitionCurveType transitionCurveType)
			: base(db, startDistAlong, horizontalLength, startCantLeft, startCantRight)
		{
			IsStartRadiusCCW = isStartRadiusCCW;
			IsEndRadiusCCW = isEndRadiusCCW;
			TransitionCurveType = transitionCurveType;
		}
	}
	[Serializable]
	public partial class IfcAlignment2DVerSegTransition : IfcAlignment2DVerticalSegment
	{
		private double mStartRadius = double.NaN; //: OPTIONAL IfcPositiveLengthMeasure;
		private double mEndRadius = double.NaN; //: OPTIONAL IfcPositiveLengthMeasure;
		private bool mIsStartRadiusCCW = false; //: IfcBoolean;
		private bool mIsEndRadiusCCW = false; //: IfcBoolean;
		private IfcTransitionCurveType mTransitionCurveType = IfcTransitionCurveType.BIQUADRATICPARABOLA; //: IfcTransitionCurveType;

		public double StartRadius { get { return mStartRadius; } set { mStartRadius = value; } }
		public double EndRadius { get { return mEndRadius; } set { mEndRadius = value; } }
		public bool IsStartRadiusCCW { get { return mIsStartRadiusCCW; } set { mIsStartRadiusCCW = value; } }
		public bool IsEndRadiusCCW { get { return mIsEndRadiusCCW; } set { mIsEndRadiusCCW = value; } }
		public IfcTransitionCurveType TransitionCurveType { get { return mTransitionCurveType; } set { mTransitionCurveType = value; } }

		public IfcAlignment2DVerSegTransition() : base() { }
		internal IfcAlignment2DVerSegTransition(DatabaseIfc db, IfcAlignment2DVerSegTransition alignment2DVerSegTransition, DuplicateOptions options) : base(db, alignment2DVerSegTransition, options)
		{
			StartRadius = alignment2DVerSegTransition.StartRadius;
			EndRadius = alignment2DVerSegTransition.EndRadius;
			IsStartRadiusCCW = alignment2DVerSegTransition.IsStartRadiusCCW;
			IsEndRadiusCCW = alignment2DVerSegTransition.IsEndRadiusCCW;
			TransitionCurveType = alignment2DVerSegTransition.TransitionCurveType;
		}
		public IfcAlignment2DVerSegTransition(DatabaseIfc db, double startDistAlong, double horizontalLength, double startHeight, double startGradient, bool isStartRadiusCCW, bool isEndRadiusCCW, IfcTransitionCurveType transitionCurveType)
			: base(db, startDistAlong, horizontalLength, startHeight, startGradient)
		{
			IsStartRadiusCCW = isStartRadiusCCW;
			IsEndRadiusCCW = isEndRadiusCCW;
			TransitionCurveType = transitionCurveType;
		}
	}
	[Serializable]
	public partial class IfcAlignment2DHorizontal : IfcGeometricRepresentationItem //IFC4.1
	{
		internal double mStartDistAlong = double.NaN;// : OPTIONAL IfcLengthMeasure;
		internal LIST<IfcAlignment2DHorizontalSegment> mSegments = new LIST<IfcAlignment2DHorizontalSegment>();// : LIST [1:?] OF IfcAlignment2DHorizontalSegment;
	   //INVERSE
		internal SET<IfcAlignmentCurve> mToAlignmentCurve = new SET<IfcAlignmentCurve>();// : SET[1:?] OF IfcAlignmentCurve FOR Horizontal;

		public double StartDistAlong { get { return double.IsNaN(mStartDistAlong) ? 0 : mStartDistAlong; } set { mStartDistAlong = value; } }
		public LIST<IfcAlignment2DHorizontalSegment> Segments { get { return mSegments; } set { mSegments = value; } }
		public SET<IfcAlignmentCurve> ToAlignmentCurve { get { return mToAlignmentCurve; } set { mToAlignmentCurve = value;  } } 

		internal IfcAlignment2DHorizontal() : base() { }
		internal IfcAlignment2DHorizontal(DatabaseIfc db, IfcAlignment2DHorizontal a, DuplicateOptions options) : base(db, a, options)
		{
			mStartDistAlong = a.mStartDistAlong;
			Segments.AddRange(a.Segments.ConvertAll(x => db.Factory.Duplicate(x) as IfcAlignment2DHorizontalSegment));
		}
		public IfcAlignment2DHorizontal(IEnumerable<IfcAlignment2DHorizontalSegment> segments) : base(segments.First().Database) { mSegments.AddRange(segments); }
		protected override void initialize()
		{
			base.initialize();
			mSegments.CollectionChanged += mSegments_CollectionChanged;
		}
		private void mSegments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (mDatabase != null && mDatabase.IsDisposed())
				return;
			if (e.NewItems != null)
			{
				foreach (IfcAlignment2DHorizontalSegment segment in e.NewItems)
					segment.ToHorizontal = this;
			}
			if (e.OldItems != null)
			{
				foreach (IfcAlignment2DHorizontalSegment segment in e.OldItems)
					segment.ToHorizontal = null;
			}
		}
	}
	[Serializable]
	public partial class IfcAlignment2DHorizontalSegment : IfcAlignment2DSegment //IFC4.1
	{
		private IfcCurveSegment2D mCurveGeometry;// : IfcCurveSegment2D;
		//INVERSE
		internal IfcAlignment2DHorizontal mToHorizontal = null;

		public IfcCurveSegment2D CurveGeometry { get { return mCurveGeometry; } set { if (mCurveGeometry != null) mCurveGeometry.mToSegment = null; mCurveGeometry = value; if (value != null) mCurveGeometry.mToSegment = this; } }
		public IfcAlignment2DHorizontal ToHorizontal { get { return mToHorizontal; } set { mToHorizontal = value; } }

		internal IfcAlignment2DHorizontalSegment() : base() { }
		internal IfcAlignment2DHorizontalSegment(DatabaseIfc db, IfcAlignment2DHorizontalSegment s, DuplicateOptions options) : base(db, s, options) { CurveGeometry = db.Factory.Duplicate(s.CurveGeometry) as IfcCurveSegment2D; }
		public IfcAlignment2DHorizontalSegment(IfcCurveSegment2D seg) : base(seg.mDatabase) { CurveGeometry = seg; }
	}
	[Serializable]
	public abstract partial class IfcAlignment2DSegment : IfcGeometricRepresentationItem //IFC4.1 ABSTRACT SUPERTYPE OF(ONEOF(IfcAlignment2DHorizontalSegment, IfcAlignment2DVerticalSegment))
	{
		internal IfcLogicalEnum mTangentialContinuity = IfcLogicalEnum.UNKNOWN;// : OPTIONAL IfcBoolean;
		private string mStartTag = "$";// : OPTIONAL IfcLabel;
		private string mEndTag = "$";// : OPTIONAL IfcLabel;

		public bool TangentialContinuity { get { return mTangentialContinuity == IfcLogicalEnum.TRUE; } set { mTangentialContinuity = (value ? IfcLogicalEnum.TRUE : IfcLogicalEnum.FALSE); } }
		public string StartTag { get { return (mStartTag == "$" ? "" : ParserIfc.Decode(mStartTag)); } set { mStartTag = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value.Replace("'", ""))); } }
		public string EndTag { get { return (mEndTag == "$" ? "" : ParserIfc.Decode(mEndTag)); } set { mEndTag = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value.Replace("'", ""))); } }

		protected IfcAlignment2DSegment() : base() { }
		protected IfcAlignment2DSegment(DatabaseIfc db) : base(db) { }
		protected IfcAlignment2DSegment(DatabaseIfc db, IfcAlignment2DSegment s, DuplicateOptions options) : base(db, s, options)
		{
			TangentialContinuity = s.TangentialContinuity;
			StartTag = s.StartTag;
			EndTag = s.EndTag;
		}
	}
	[Serializable]
	public partial class IfcAlignment2DVerSegCircularArc : IfcAlignment2DVerticalSegment  //IFC4.1
	{
		private double mRadius;// : IfcPositiveLengthMeasure;
		private bool mIsConvex;// : IfcBoolean;

		public double Radius { get { return mRadius; } set { mRadius = value; } }
		public bool IsConvex { get { return mIsConvex; } set { mIsConvex = value; } }

		internal IfcAlignment2DVerSegCircularArc() : base() { }
		internal IfcAlignment2DVerSegCircularArc(DatabaseIfc db, IfcAlignment2DVerSegCircularArc alignment2DVerSegCircularArc, DuplicateOptions options) : base(db, alignment2DVerSegCircularArc, options)
		{
			Radius = alignment2DVerSegCircularArc.Radius;
			IsConvex = alignment2DVerSegCircularArc.IsConvex;
		}
		public IfcAlignment2DVerSegCircularArc(DatabaseIfc db, double startDist, double horizontalLength, double startHeight, double startGradient, double radius, bool isConvex)
			: base(db, startDist, horizontalLength, startHeight, startGradient)
		{
			mRadius = radius;
			mIsConvex = isConvex;
		}
	}
	[Serializable]
	public partial class IfcAlignment2DVerSegLine : IfcAlignment2DVerticalSegment  //IFC4.1
	{
		internal IfcAlignment2DVerSegLine() : base() { }
		internal IfcAlignment2DVerSegLine(DatabaseIfc db, IfcAlignment2DVerSegLine alignment2DVerSegLine, DuplicateOptions options) : base(db, alignment2DVerSegLine, options) { }
		public IfcAlignment2DVerSegLine(DatabaseIfc db, double startDist, double horizontalLength, double startHeight, double startGradient)
			: base(db, startDist, horizontalLength, startHeight, startGradient) { }
	}
	[Serializable]
	public partial class IfcAlignment2DVerSegParabolicArc : IfcAlignment2DVerticalSegment  //IFC4.1
	{
		private double mParabolaConstant;// : IfcPositiveLengthMeasure;
		private bool mIsConvex;// : IfcBoolean;

		public double ParabolaConstant { get { return mParabolaConstant; } set { mParabolaConstant = value; } }
		public bool IsConvex { get { return mIsConvex; } set { mIsConvex = value; } }

		internal IfcAlignment2DVerSegParabolicArc() : base() { }
		internal IfcAlignment2DVerSegParabolicArc(DatabaseIfc db, IfcAlignment2DVerSegParabolicArc alignment2DVerSegParabolicArc, DuplicateOptions options) : base(db, alignment2DVerSegParabolicArc, options)
		{
			ParabolaConstant = alignment2DVerSegParabolicArc.ParabolaConstant;
			IsConvex = alignment2DVerSegParabolicArc.IsConvex;
		}
		public IfcAlignment2DVerSegParabolicArc(DatabaseIfc db, double startDist, double horizontalLength, double startHeight, double startGradient, double parabolaConstant, bool isConvex)
			: base(db, startDist, horizontalLength, startHeight, startGradient)
		{
			mParabolaConstant = parabolaConstant;
			mIsConvex = isConvex;
		}
	}
	[Serializable]
	public partial class IfcAlignment2DVertical : IfcGeometricRepresentationItem //IFC4.1
	{
		internal LIST<IfcAlignment2DVerticalSegment> mSegments = new LIST<IfcAlignment2DVerticalSegment>();// : LIST [1:?] OF IfcAlignment2DVerticalSegment;
		//INVERSE
		internal IfcAlignmentCurve mToAlignmentCurve = null;// : SET[1:1] OF IfcAlignmentCurve FOR Vertical;
		public LIST<IfcAlignment2DVerticalSegment> Segments { get { return mSegments; } set { mSegments = value; } }
		public IfcAlignmentCurve ToAlignmentCurve { get { return mToAlignmentCurve; } set { mToAlignmentCurve = value; } }// : SET[1:1] OF IfcAlignmentCurve FOR Vertical;
	
		internal IfcAlignment2DVertical() : base() { }
		internal IfcAlignment2DVertical(DatabaseIfc db, IfcAlignment2DVertical a, DuplicateOptions options) : base(db, a, options)
		{
			Segments.AddRange(a.Segments.ConvertAll(x => db.Factory.Duplicate(x) as IfcAlignment2DVerticalSegment));
		}
		public IfcAlignment2DVertical(IEnumerable<IfcAlignment2DVerticalSegment> segments) : base(segments.First().Database) { Segments.AddRange(segments); }
	}
	[Serializable]
	public abstract partial class IfcAlignment2DVerticalSegment : IfcAlignment2DSegment //IFC4.1
	{
		internal double mStartDistAlong;// : IfcLengthMeasure;
		internal double mHorizontalLength;// : IfcPositiveLengthMeasure;
		internal double mStartHeight;// : IfcLengthMeasure;
		internal double mStartGradient;// : IfcRatioMeasure; 

		public double StartDistAlong { get { return mStartDistAlong; } set { mStartDistAlong = value; } }
		public double HorizontalLength { get { return mHorizontalLength; } set { mHorizontalLength = value; } }
		public double StartHeight { get { return mStartHeight; } set { mStartHeight = value; } }
		public double StartGradient { get { return mStartGradient; } set { mStartGradient = value; } }

		protected IfcAlignment2DVerticalSegment() : base() { }
		protected IfcAlignment2DVerticalSegment(DatabaseIfc db, IfcAlignment2DVerticalSegment alignment2DVerticalSegment, DuplicateOptions options) : base(db, alignment2DVerticalSegment, options)
		{
			StartDistAlong = alignment2DVerticalSegment.StartDistAlong;
			HorizontalLength = alignment2DVerticalSegment.HorizontalLength;
			StartHeight = alignment2DVerticalSegment.StartHeight;
			StartGradient = alignment2DVerticalSegment.StartGradient;
		}
		protected IfcAlignment2DVerticalSegment(DatabaseIfc db, double startDist, double horizontalLength, double startHeight, double startGradient) : base(db)
		{
			mStartDistAlong = startDist;
			mHorizontalLength = horizontalLength;
			mStartHeight = startHeight;
			mStartGradient = startGradient;
		}
	}
	[Serializable]
	public partial class IfcAlignmentCurve : IfcBoundedCurve //IFC4.1
	{
		internal IfcAlignment2DHorizontal mHorizontal = null;// : OPTIONAL IfcAlignment2DHorizontal;
		private IfcAlignment2DVertical mVertical = null;// : OPTIONAL IfcAlignment2DVertical;
		internal string mTag = "$";// : OPTIONAL IfcLabel;

		public IfcAlignment2DHorizontal Horizontal { get { return mHorizontal; } set { mHorizontal = value; } }
		public IfcAlignment2DVertical Vertical { get { return mVertical; } set { if (mVertical != null) mVertical.ToAlignmentCurve = null; mVertical = value; if (value != null) value.ToAlignmentCurve = this; } }
		public string Tag { get { return (mTag == "$" ? "" : ParserIfc.Decode(mTag)); } set { mTag = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }

		internal IfcAlignmentCurve() : base() { }
		internal IfcAlignmentCurve(DatabaseIfc db, IfcAlignmentCurve c, DuplicateOptions options) : base(db, c, options)
		{
			if (c.mHorizontal != null)
				Horizontal = db.Factory.Duplicate(c.Horizontal) as IfcAlignment2DHorizontal;
			if (c.mVertical != null)
				Vertical = db.Factory.Duplicate(c.Vertical) as IfcAlignment2DVertical;
			Tag = c.Tag;
		}
		public IfcAlignmentCurve(DatabaseIfc db) : base(db) { }
		public IfcAlignmentCurve(IfcAlignment2DHorizontal horizontal) : base(horizontal.Database) { Horizontal = horizontal; }
		public IfcAlignmentCurve(IfcAlignment2DVertical vertical) : base(vertical.Database) { Vertical = vertical; }
		public IfcAlignmentCurve(IfcAlignment2DHorizontal horizontal, IfcAlignment2DVertical vertical) : this(horizontal) { Vertical = vertical; }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcAngularDimension : IfcDimensionCurveDirectedCallout //IFC4 DEPRECATED
	{
		internal IfcAngularDimension() : base() { }
	}
	[Serializable]
	public partial class IfcAnnotation : IfcProduct
	{    
		internal IfcAnnotationTypeEnum mPredefinedType = IfcAnnotationTypeEnum.NOTDEFINED;//: OPTIONAL IfcBeamTypeEnum; IFC4
		public IfcAnnotationTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAnnotation() : base() { }
		public IfcAnnotation(DatabaseIfc db) : base(db) { }
		internal IfcAnnotation(DatabaseIfc db, IfcAnnotation a, DuplicateOptions options) : base(db, a, options) { }
		public IfcAnnotation(IfcProduct host) : base(host.mDatabase) { host.AddElement(this); }

		internal override void detachFromHost()
		{
			base.detachFromHost();
			if (mContainedInStructure != null)
			{
				mContainedInStructure.RelatedElements.Remove(this);
				mContainedInStructure = null;
			}
		}
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public abstract partial class IfcAnnotationCurveOccurrence : IfcAnnotationOccurrence //IFC4 DEPRECATED
	{
		protected IfcAnnotationCurveOccurrence() : base() { }
	}
	[Serializable]
	public partial class IfcAnnotationFillArea : IfcGeometricRepresentationItem  
	{
		internal IfcCurve mOuterBoundary;// : IfcCurve;
		internal SET<IfcCurve> mInnerBoundaries = new SET<IfcCurve>();// OPTIONAL SET [1:?] OF IfcCurve; 

		public IfcCurve OuterBoundary { get { return mOuterBoundary; } set { mOuterBoundary = value; } }
		public SET<IfcCurve> InnerBoundaries { get { return mInnerBoundaries; } }

		internal IfcAnnotationFillArea() : base() { }
		internal IfcAnnotationFillArea(DatabaseIfc db, IfcAnnotationFillArea a, DuplicateOptions options) : base(db, a, options) { OuterBoundary = a.OuterBoundary.Duplicate(db, options) as IfcCurve; InnerBoundaries.AddRange(a.InnerBoundaries.ConvertAll(x=> db.Factory.Duplicate(x) as IfcCurve)); }
		public IfcAnnotationFillArea(IfcCurve outerBoundary) : base(outerBoundary.mDatabase) { OuterBoundary = outerBoundary; }
		public IfcAnnotationFillArea(IfcCurve outerBoundary, List<IfcCurve> innerBoundaries) : this(outerBoundary) { InnerBoundaries.AddRange(innerBoundaries); }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcAnnotationFillAreaOccurrence : IfcAnnotationOccurrence //IFC4 DEPRECATED
	{
		internal IfcPoint mFillStyleTarget;// : OPTIONAL IfcPoint;
		internal IfcGlobalOrLocalEnum  mGlobalOrLocal;// : OPTIONAL IfcGlobalOrLocalEnum; 

		internal IfcAnnotationFillAreaOccurrence() : base() { }
		internal IfcAnnotationFillAreaOccurrence(DatabaseIfc db, IfcAnnotationFillAreaOccurrence f, DuplicateOptions options) : base(db, f, options) { }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public abstract partial class IfcAnnotationOccurrence : IfcStyledItem //DEPRECATED IFC4
	{
		protected IfcAnnotationOccurrence(DatabaseIfc db, IfcAnnotationOccurrence o, DuplicateOptions options) : base(db, o, options) { }
		protected IfcAnnotationOccurrence() : base() { }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcAnnotationSurface : IfcGeometricRepresentationItem //DEPRECATED IFC4
	{
		internal IfcGeometricRepresentationItem mItem;// : IfcGeometricRepresentationItem;
		internal IfcTextureCoordinate mTextureCoordinates;// OPTIONAL IfcTextureCoordinate;

		public IfcGeometricRepresentationItem Item { get { return mItem; } set { mItem = value; } }
		public IfcTextureCoordinate TextureCoordinates { get { return mTextureCoordinates; } set { mTextureCoordinates = value; } }

		internal IfcAnnotationSurface() : base() { } 
		internal IfcAnnotationSurface(DatabaseIfc db, IfcAnnotationSurface a, DuplicateOptions options) : base(db, a, options) { Item = a.Item.Duplicate(db, options) as IfcGeometricRepresentationItem; if(a.mTextureCoordinates != null) TextureCoordinates = db.Factory.Duplicate(a.TextureCoordinates) as IfcTextureCoordinate; }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcAnnotationSurfaceOccurrence : IfcAnnotationOccurrence //IFC4 DEPRECATED
	{
		internal IfcAnnotationSurfaceOccurrence() : base() { }
		internal IfcAnnotationSurfaceOccurrence(DatabaseIfc db, IfcAnnotationSurfaceOccurrence o, DuplicateOptions options) : base(db, o, options) { }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcAnnotationSymbolOccurrence : IfcAnnotationOccurrence //IFC4 DEPRECATED
	{
		internal IfcAnnotationSymbolOccurrence() : base() { }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcAnnotationTextOccurrence : IfcAnnotationOccurrence //IFC4 DEPRECATED
	{
		internal IfcAnnotationTextOccurrence() : base() { }
		internal IfcAnnotationTextOccurrence(DatabaseIfc db, IfcAnnotationTextOccurrence o, DuplicateOptions options) : base(db, o, options) { }
	}
	[Serializable]
	public partial class IfcApplication : BaseClassIfc, NamedObjectIfc
	{
		internal IfcOrganization mApplicationDeveloper = null;// : IfcOrganization;
		internal string mVersion;// : IfcLabel;
		private string mApplicationFullName;// : IfcLabel;
		internal string mApplicationIdentifier;// : IfcIdentifier; 
		
		public IfcOrganization ApplicationDeveloper { get { return mApplicationDeveloper; } set { mApplicationDeveloper = value; } }
		public string Version { get { return mVersion; } set { mVersion = ParserIfc.Encode(value); } }
		public string ApplicationFullName { get { return ParserIfc.Decode(mApplicationFullName); } set { mApplicationFullName =  ParserIfc.Encode(value); } }
		public string ApplicationIdentifier { get { return ParserIfc.Decode(mApplicationIdentifier); } set { mApplicationIdentifier =  ParserIfc.Encode(value); } }

		public string Name { get { return ApplicationFullName; } set { ApplicationFullName = value; } }

		internal IfcApplication() : base() { }
		internal IfcApplication(DatabaseIfc db) : base(db)
		{
			ApplicationDeveloper = new IfcOrganization(db, "Geometry Gym Pty Ltd");
			try
			{
				mVersion =  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
			catch (Exception) { mVersion = "Unknown"; }
			mApplicationFullName = db.Factory.ApplicationFullName;
			mApplicationIdentifier = db.Factory.ApplicationIdentifier;

		}
		internal IfcApplication(DatabaseIfc db, IfcApplication a) : base(db,a)
		{
			ApplicationDeveloper = db.Factory.Duplicate(a.ApplicationDeveloper) as IfcOrganization;
			mVersion = a.mVersion;
			mApplicationFullName = a.mApplicationFullName;
			mApplicationIdentifier = a.mApplicationIdentifier;
		}
		public IfcApplication(IfcOrganization developer, string version, string fullName, string identifier) :base(developer.mDatabase) { ApplicationDeveloper = developer; Version = version; ApplicationFullName = fullName; ApplicationIdentifier = identifier; }
	}
	[Serializable]
	public partial class IfcAppliedValue : BaseClassIfc, IfcMetricValueSelect, IfcObjectReferenceSelect, IfcResourceObjectSelect, NamedObjectIfc
	{  // SUPERTYPE OF(IfcCostValue);
		internal string mName = "$";// : OPTIONAL IfcLabel;
		internal string mDescription = "$";// : OPTIONAL IfcText;
		internal IfcAppliedValueSelect mAppliedValue = null;// : OPTIONAL IfcAppliedValueSelect;
		internal int mUnitBasis;// : OPTIONAL IfcMeasureWithUnit;
		internal DateTime mApplicableDate = DateTime.MinValue;// : OPTIONAL IfcDateTimeSelect; 4 IfcDate
		internal DateTime mFixedUntilDate = DateTime.MinValue;// : OPTIONAL IfcDateTimeSelect; 4 IfcDate
		private IfcDateTimeSelect mSSApplicableDate = null;
		private IfcDateTimeSelect mSSFixedUntilDate = null;
		internal string mCategory = "$";// : OPTIONAL IfcLabel; IFC4
		internal string mCondition = "$";// : OPTIONAL IfcLabel; IFC4
		internal IfcArithmeticOperatorEnum mArithmeticOperator = IfcArithmeticOperatorEnum.NONE;//	 :	OPTIONAL IfcArithmeticOperatorEnum; IFC4 
		internal List<int> mComponents = new List<int>();//	 :	OPTIONAL LIST [1:?] OF IfcAppliedValue; IFC4
		//INVERSE
		private SET<IfcExternalReferenceRelationship> mHasExternalReference = new SET<IfcExternalReferenceRelationship>(); //IFC4 SET [0:?] OF IfcExternalReferenceRelationship FOR RelatedResourceObjects;
		internal List<IfcResourceConstraintRelationship> mHasConstraintRelationships = new List<IfcResourceConstraintRelationship>(); //gg
		internal List<IfcAppliedValue> mComponentFor = new List<IfcAppliedValue>(); //gg

		public string Name { get { return (mName == "$" ? "" : ParserIfc.Decode(mName)); } set { mName = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } } 
		public string Description { get { return (mDescription == "$" ? "" : ParserIfc.Decode(mDescription)); } set { mDescription = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		public IfcAppliedValueSelect AppliedValue { get { return mAppliedValue; } set { mAppliedValue = value; } }
		public IfcMeasureWithUnit UnitBasis { get { return mDatabase[mUnitBasis] as IfcMeasureWithUnit; } set { mUnitBasis = (value == null ? 0 : value.mIndex); } }
		public DateTime ApplicableDate { get { return mApplicableDate; } set { mApplicableDate = value; } }
		public DateTime FixedUntilDate { get { return mFixedUntilDate; } set { mFixedUntilDate = value; } }
		public string Category { get { return (mCategory == "$" ? "" : ParserIfc.Decode(mCategory)); } set { mCategory = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		public string Condition { get { return (mCondition == "$" ? "" : ParserIfc.Decode(mCondition)); } set { mCondition = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		public IfcArithmeticOperatorEnum ArithmeticOperator { get { return mArithmeticOperator; } set { mArithmeticOperator = value; } }
		public ReadOnlyCollection<IfcAppliedValue> Components { get { return new ReadOnlyCollection<IfcAppliedValue>( mComponents.ConvertAll(x => mDatabase[x] as IfcAppliedValue)); } }
		public SET<IfcExternalReferenceRelationship> HasExternalReference { get { return mHasExternalReference; } set { mHasExternalReference.Clear();  if (value != null) { mHasExternalReference.CollectionChanged -= mHasExternalReference_CollectionChanged; mHasExternalReference = value; mHasExternalReference.CollectionChanged += mHasExternalReference_CollectionChanged; } } }
		public ReadOnlyCollection<IfcResourceConstraintRelationship> HasConstraintRelationships { get { return new ReadOnlyCollection<IfcResourceConstraintRelationship>(mHasConstraintRelationships); } }

		internal IfcAppliedValue() : base() { }
		internal IfcAppliedValue(DatabaseIfc db, IfcAppliedValue v) : base(db, v)
		{
			mName = v.mName;
			mDescription = v.mDescription;
			if(v.mAppliedValue != null)
			{
				IfcValue value = v.mAppliedValue as IfcValue;
				if (value != null)
					mAppliedValue = value;
				else
					AppliedValue = db.Factory.Duplicate(v.mAppliedValue) as IfcAppliedValueSelect;
			}
			UnitBasis = db.Factory.Duplicate(v.UnitBasis) as IfcMeasureWithUnit;
			mApplicableDate = v.mApplicableDate; mFixedUntilDate = v.mFixedUntilDate; mCategory = v.mCategory; mCondition = v.mCondition; mArithmeticOperator = v.mArithmeticOperator;
			v.Components.ToList().ForEach(x => addComponent(db.Factory.Duplicate(x) as IfcAppliedValue));
		}
		public IfcAppliedValue(DatabaseIfc db) : base(db) { }
		public IfcAppliedValue(IfcAppliedValueSelect appliedValue) : base(appliedValue.Database) { AppliedValue = appliedValue; }
		public IfcAppliedValue(DatabaseIfc db, IfcValue value) : base(db) { AppliedValue = value; }
		public IfcAppliedValue(IfcAppliedValue component1, IfcArithmeticOperatorEnum op,IfcAppliedValue component2) : base(component1.mDatabase) { addComponent(component1); addComponent(component2); mArithmeticOperator = op; } 
		

		protected override void initialize()
		{
			base.initialize();

			mHasExternalReference.CollectionChanged += mHasExternalReference_CollectionChanged;
		}
		private void mHasExternalReference_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (mDatabase != null && mDatabase.IsDisposed())
				return;
			if (e.NewItems != null)
			{
				foreach (IfcExternalReferenceRelationship r in e.NewItems)
				{
					if (!r.RelatedResourceObjects.Contains(this))
						r.RelatedResourceObjects.Add(this);
				}
			}
			if (e.OldItems != null)
			{
				foreach (IfcExternalReferenceRelationship r in e.OldItems)
					r.RelatedResourceObjects.Remove(this);
			}
		}

		protected override bool DisposeWorker(bool children)
		{
			if (children)
			{
				BaseClassIfc value = mAppliedValue as BaseClassIfc;
				if(value != null)
					value.Dispose(children);
				for (int icounter = 0; icounter < mComponents.Count; icounter++)
				{
					BaseClassIfc bc = mDatabase[mComponents[icounter]];
					if (bc != null)
						bc.Dispose(true);
				}
			}
			return base.DisposeWorker(children);
		}

		public void AddConstraintRelationShip(IfcResourceConstraintRelationship constraintRelationship) { mHasConstraintRelationships.Add(constraintRelationship); }
		internal void addComponent(IfcAppliedValue value) { mComponents.Add(value.mIndex); value.mComponentFor.Add(this); }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcAppliedValueRelationship : BaseClassIfc //DEPRECATED IFC4
	{
		internal IfcAppliedValue mComponentOfTotal;// : IfcAppliedValue;
		internal SET<IfcAppliedValue> mComponents = new SET<IfcAppliedValue>();// : SET [1:?] OF IfcAppliedValue;
		internal IfcArithmeticOperatorEnum mArithmeticOperator;// : IfcArithmeticOperatorEnum;
		internal string mName;// : OPTIONAL IfcLabel;
		internal string mDescription;// : OPTIONAL IfcText 

		public IfcAppliedValue ComponentOfTotal { get { return mComponentOfTotal; } set { mComponentOfTotal = value; } }
		public SET<IfcAppliedValue> Components { get { return mComponents; } set{ mComponents = value; } } 

		internal IfcAppliedValueRelationship() : base() { }
		//internal IfcAppliedValueRelationship(IfcAppliedValueRelationship o) : base()
		//{
		//	mComponentOfTotal = o.mComponentOfTotal;
		//	mComponents = new List<int>(o.mComponents.ToArray());
		//	mArithmeticOperator = o.mArithmeticOperator;
		//	mName = o.mName;
		//	mDescription = o.mDescription;
		//}
	}
	public interface IfcAppliedValueSelect : IBaseClassIfc  //	IfcMeasureWithUnit, IfcValue, IfcReference); IFC2x3 //IfcRatioMeasure, IfcMeasureWithUnit, IfcMonetaryMeasure); 
	{
		//List<IfcAppliedValue> AppliedValueFor { get; }
	}
	[Serializable]
	public partial class IfcApproval : BaseClassIfc, IfcResourceObjectSelect, NamedObjectIfc
	{
		internal string mDescription = "$";// : OPTIONAL IfcText;
		internal int mApprovalDateTime;// : IfcDateTimeSelect;
		internal string mApprovalStatus = "$";// : OPTIONAL IfcLabel;
		internal string mApprovalLevel = "$";// : OPTIONAL IfcLabel;
		internal string mApprovalQualifier = "$";// : OPTIONAL IfcText;
		internal string mName = "$";// :OPTIONAL IfcLabel;
		internal string mIdentifier = "$";// : OPTIONAL IfcIdentifier;
										  //INVERSE
		private SET<IfcExternalReferenceRelationship> mHasExternalReference = new SET<IfcExternalReferenceRelationship>(); //IFC4 SET [0:?] OF IfcExternalReferenceRelationship FOR RelatedResourceObjects;
		private SET<IfcRelAssociatesApproval> mApprovedObjects = new SET<IfcRelAssociatesApproval>();
		internal List<IfcResourceConstraintRelationship> mHasConstraintRelationships = new List<IfcResourceConstraintRelationship>(); //gg

		public string Name { get { return (mName == "$" ? "" : ParserIfc.Decode(mName)); } set { mName = (string.IsNullOrEmpty(value) ? "$" : ParserIfc.Encode(value)); } }
		public SET<IfcExternalReferenceRelationship> HasExternalReference { get { return mHasExternalReference; } set { mHasExternalReference.Clear();  if (value != null) { mHasExternalReference.CollectionChanged -= mHasExternalReference_CollectionChanged; mHasExternalReference = value; mHasExternalReference.CollectionChanged += mHasExternalReference_CollectionChanged; } } }
		public SET<IfcRelAssociatesApproval> ApprovedObjects { get { return mApprovedObjects; } set { mApprovedObjects.Clear();  if (value != null) { mApprovedObjects = value; } } }
		public ReadOnlyCollection<IfcResourceConstraintRelationship> HasConstraintRelationships { get { return new ReadOnlyCollection<IfcResourceConstraintRelationship>(mHasConstraintRelationships); } }

		internal IfcApproval() : base() { }
		//internal IfcApproval(IfcApproval o) : base()
		//{
		//	mDescription = o.mDescription;
		//	mApprovalDateTime = o.mApprovalDateTime;
		//	mApprovalStatus = o.mApprovalStatus;
		//	mApprovalLevel = o.mApprovalLevel;
		//	mApprovalQualifier = o.mApprovalQualifier;
		//	mName = o.mName;
		//	mIdentifier = o.mIdentifier;
		//}
		protected override void initialize()
		{
			base.initialize();

			mHasExternalReference.CollectionChanged += mHasExternalReference_CollectionChanged;
		}
		private void mHasExternalReference_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (mDatabase != null && mDatabase.IsDisposed())
				return;
			if (e.NewItems != null)
			{
				foreach (IfcExternalReferenceRelationship r in e.NewItems)
				{
					if (!r.RelatedResourceObjects.Contains(this))
						r.RelatedResourceObjects.Add(this);
				}
			}
			if (e.OldItems != null)
			{
				foreach (IfcExternalReferenceRelationship r in e.OldItems)
					r.RelatedResourceObjects.Remove(this);
			}
		}
		public void AddConstraintRelationShip(IfcResourceConstraintRelationship constraintRelationship) { mHasConstraintRelationships.Add(constraintRelationship); }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcApprovalActorRelationship : BaseClassIfc //DEPRECATED IFC4
	{
		internal int mActor;// : IfcActorSelect;
		internal int mApproval;// : IfcApproval;
		internal int mRole;// : IfcActorRole; 
		internal IfcApprovalActorRelationship() : base() { }
		//internal IfcApprovalActorRelationship(IfcApprovalActorRelationship o) : base() { mActor = o.mActor; mApproval = o.mApproval; mRole = o.mRole; }
	}
	[Obsolete("DEPRECATED IFC4", false)]
	[Serializable]
	public partial class IfcApprovalPropertyRelationship : BaseClassIfc //DEPRECATED IFC4
	{
		internal List<int> mApprovedProperties = new List<int>();// : SET [1:?] OF IfcProperty;
		internal int mApproval;// : IfcApproval; 
		internal IfcApprovalPropertyRelationship() : base() { }
		//internal IfcApprovalPropertyRelationship(IfcApprovalPropertyRelationship o) : base() { mApprovedProperties = new List<int>(o.mApprovedProperties.ToArray()); mApproval = o.mApproval; }
	}
	[Serializable]
	public partial class IfcApprovalRelationship : IfcResourceLevelRelationship //IFC4Change
	{
		internal int mRelatedApproval;// : IfcApproval;
		internal int mRelatingApproval;// : IfcApproval; 
		internal IfcApprovalRelationship() : base() { }
	//	internal IfcApprovalRelationship(IfcApprovalRelationship o) : base(o) { mRelatedApproval = o.mRelatedApproval; mRelatingApproval = o.mRelatingApproval;  }
	}
	[Serializable]
	public partial class IfcArbitraryClosedProfileDef : IfcProfileDef //SUPERTYPE OF(IfcArbitraryProfileDefWithVoids)
	{
		private int mOuterCurve;//: IfcCurve;
		public IfcCurve OuterCurve { get { return mDatabase[mOuterCurve] as IfcCurve; } set { mOuterCurve = value.mIndex; } }

		internal IfcArbitraryClosedProfileDef() : base() { }
		internal IfcArbitraryClosedProfileDef(DatabaseIfc db, IfcArbitraryClosedProfileDef p, DuplicateOptions options) 
			: base(db, p, options) { OuterCurve = p.OuterCurve.Duplicate(db, options) as IfcCurve; }
		public IfcArbitraryClosedProfileDef(string name, IfcCurve boundedCurve) : base(boundedCurve.mDatabase,name) { mOuterCurve = boundedCurve.mIndex; }//if (string.Compare(getKW, mKW) == 0) mModel.mArbProfiles.Add(this); }
	}
	[Serializable]
	public partial class IfcArbitraryOpenProfileDef : IfcProfileDef //	SUPERTYPE OF(IfcCenterLineProfileDef)
	{
		private int mCurve;// : IfcBoundedCurve
		public IfcBoundedCurve Curve { get { return mDatabase[mCurve] as IfcBoundedCurve; } set { mCurve = value.mIndex; } }

		internal IfcArbitraryOpenProfileDef() : base() { }
		internal IfcArbitraryOpenProfileDef(DatabaseIfc db, IfcArbitraryOpenProfileDef p, DuplicateOptions options) : base(db, p, options) { Curve = p.Curve.Duplicate(db, options) as IfcBoundedCurve; }
		public IfcArbitraryOpenProfileDef(string name, IfcBoundedCurve boundedCurve) : base(boundedCurve.mDatabase,name) { mCurve = boundedCurve.mIndex; mProfileType = IfcProfileTypeEnum.CURVE; }
	}
	[Serializable]
	public partial class IfcArbitraryProfileDefWithVoids : IfcArbitraryClosedProfileDef
	{
		private List<int> mInnerCurves = new List<int>();// : SET [1:?] OF IfcCurve; 
		public ReadOnlyCollection<IfcCurve> InnerCurves { get { return new ReadOnlyCollection<IfcCurve>(mInnerCurves.ConvertAll(x => mDatabase[x] as IfcCurve)); } }

		internal IfcArbitraryProfileDefWithVoids() : base() { }
		internal IfcArbitraryProfileDefWithVoids(DatabaseIfc db, IfcArbitraryProfileDefWithVoids p, DuplicateOptions options) : base(db, p, options) 
		{ 
			p.InnerCurves.ToList().ForEach(x => addVoid(x.Duplicate(db, options) as IfcCurve));
		}
		public IfcArbitraryProfileDefWithVoids(string name, IfcCurve perim, IfcCurve inner) : base(name, perim) { mInnerCurves.Add(inner.mIndex); }
		public IfcArbitraryProfileDefWithVoids(string name, IfcCurve perim, IEnumerable<IfcCurve> inner) : base(name, perim) { mInnerCurves.AddRange(inner.Select(x=>x.StepId)); }
		
		internal void addVoid(IfcCurve inner) { mInnerCurves.Add(inner.mIndex); }
	}
	[Serializable]
	public partial class IfcArcIndex : List<int>, IfcSegmentIndexSelect
	{
		public IfcArcIndex(int a, int b, int c) { Add(a); Add(b); Add(c); }
		public override string ToString() { return "IFCARCINDEX((" + this[0] + "," + this[1] + "," + this[2] + "))"; }
	}
	[Serializable]
	public partial class IfcAsset : IfcGroup
	{
		internal string mIdentification = "";// ifc2x3 AssetID; : OPTIONAL IfcIdentifier;
		internal int mOriginalValue;// : OPTIONAL IfcCostValue;
		internal int mCurrentValue;// : OPTIONAL IfcCostValue;
		internal int mTotalReplacementCost;// : OPTIONAL IfcCostValue;
		internal int mOwner;// : IfcActorSelect;
		internal int mUser;// : IfcActorSelect;
		internal int mResponsiblePerson;// : IfcPerson;
		internal DateTime mIncorporationDate = DateTime.MinValue; // : IfcDate 
		internal int mIncorporationDateSS;// : IfcDate Ifc2x3 IfcCalendarDate;
		internal int mDepreciatedValue;// : IfcCostValue; 

		public string Identification { get { return mIdentification; } set { mIdentification = value; } }
		public IfcCostValue OriginalValue { get { return mDatabase[mOriginalValue] as IfcCostValue; } set { mOriginalValue = value.mIndex; } } 
		public IfcCostValue CurrentValue { get { return mDatabase[mCurrentValue] as IfcCostValue; } set { mCurrentValue = value.mIndex; } } 
		public IfcCostValue TotalReplacementCost { get { return mDatabase[mTotalReplacementCost] as IfcCostValue; } set { mTotalReplacementCost = value.mIndex; } } 
		public IfcActorSelect Owner { get { return mDatabase[mOwner] as IfcActorSelect; } set { mOwner = value.Index; } }
		public IfcActorSelect User { get { return mDatabase[mUser] as IfcActorSelect; } set { mUser = value.Index; } }
		public IfcPerson ResponsiblePerson { get { return mDatabase[mResponsiblePerson] as IfcPerson; } set { mResponsiblePerson = value.mIndex; } }
		//public  IncorporationDate
		public IfcCostValue DepreciatedValue { get { return mDatabase[mDepreciatedValue] as IfcCostValue; } set { mDepreciatedValue = value.mIndex; } } 

		
		internal IfcAsset() : base() { }
		internal IfcAsset(DatabaseIfc db, IfcAsset a, DuplicateOptions options) : base(db, a, options)
		{
			mIdentification = a.mIdentification;
			OriginalValue = db.Factory.Duplicate(a.OriginalValue) as IfcCostValue;
			CurrentValue = db.Factory.Duplicate(a.CurrentValue) as IfcCostValue;
			TotalReplacementCost = db.Factory.Duplicate(a.TotalReplacementCost) as IfcCostValue;
			Owner = db.Factory.Duplicate(a.mDatabase[a.mOwner]) as IfcActorSelect;
			User = db.Factory.Duplicate(a.mDatabase[a.mUser]) as IfcActorSelect;
			ResponsiblePerson = db.Factory.Duplicate(a.ResponsiblePerson) as IfcPerson;
			mIncorporationDate = a.mIncorporationDate;
			if(a.mIncorporationDateSS > 0)
				mIncorporationDateSS = db.Factory.Duplicate(a.mDatabase[ a.mIncorporationDateSS]).mIndex;

			DepreciatedValue =  db.Factory.Duplicate(a.DepreciatedValue) as IfcCostValue;
		}
		public IfcAsset(DatabaseIfc m, string name) : base(m,name) { }
	}
	[Serializable]
	public partial class IfcAsymmetricIShapeProfileDef : IfcParameterizedProfileDef // Ifc2x3 IfcIShapeProfileDef 
	{
		internal double mBottomFlangeWidth, mOverallDepth, mWebThickness, mBottomFlangeThickness;//	:	IfcPositiveLengthMeasure;
		internal double mBottomFlangeFilletRadius = double.NaN;//	:	OPTIONAL IfcNonNegativeLengthMeasure;
		internal double mTopFlangeWidth;// : IfcPositiveLengthMeasure;
		internal double mTopFlangeThickness = double.NaN;// : OPTIONAL IfcPositiveLengthMeasure;
		internal double mTopFlangeFilletRadius = double.NaN;// 	:	OPTIONAL IfcNonNegativeLengthMeasure;
		internal double mBottomFlangeEdgeRadius = double.NaN;//	:	OPTIONAL IfcNonNegativeLengthMeasure;
		internal double mBottomFlangeSlope = double.NaN;//	:	OPTIONAL IfcPlaneAngleMeasure;
		internal double mTopFlangeEdgeRadius = double.NaN;//	:	OPTIONAL IfcNonNegativeLengthMeasure;
		internal double mTopFlangeSlope = double.NaN;//:	OPTIONAL IfcPlaneAngleMeasure;
		internal double mCentreOfGravityInY;// : OPTIONAL IfcPositiveLengthMeasure IFC4 deleted

		public double BottomFlangeWidth { get { return mBottomFlangeWidth; } set { mBottomFlangeWidth = value; } }
		public double OverallDepth { get { return mOverallDepth; } set { mOverallDepth = value; } }
		public double WebThickness { get { return mWebThickness; } set { mWebThickness = value; } }
		public double BottomFlangeThickness { get { return mBottomFlangeThickness; } set { mBottomFlangeThickness = value; } }
		public double BottomFlangeFilletRadius { get { return mBottomFlangeFilletRadius; } set { mBottomFlangeFilletRadius = value; } }
		public double TopFlangeWidth { get { return mTopFlangeWidth; } set { mTopFlangeWidth = value; } }
		public double TopFlangeThickness { get { return mTopFlangeThickness; } set { mTopFlangeThickness = value; } }
		public double TopFlangeFilletRadius { get { return mTopFlangeFilletRadius; } set { mTopFlangeFilletRadius = value; } }
		public double BottomFlangeEdgeRadius { get { return mBottomFlangeEdgeRadius; } set { mBottomFlangeEdgeRadius = value; } }
		public double BottomFlangeSlope { get { return mBottomFlangeSlope; } set { mBottomFlangeSlope = value; } }
		public double TopFlangeEdgeRadius { get { return mTopFlangeEdgeRadius; } set { mTopFlangeEdgeRadius = value; } }
		public double TopFlangeSlope { get { return mTopFlangeSlope; } set { mTopFlangeSlope = value; } }

		internal IfcAsymmetricIShapeProfileDef() : base() { }
		internal IfcAsymmetricIShapeProfileDef(DatabaseIfc db, IfcAsymmetricIShapeProfileDef p, DuplicateOptions options) : base(db, p, options)
		{
			mBottomFlangeWidth = p.mBottomFlangeWidth;
			mOverallDepth = p.mOverallDepth;
			mWebThickness = p.mWebThickness;
			mBottomFlangeThickness = p.mBottomFlangeThickness;
			mBottomFlangeFilletRadius = p.mBottomFlangeFilletRadius;
			mTopFlangeWidth = p.mTopFlangeWidth;
			mTopFlangeThickness = p.mTopFlangeThickness;
			mTopFlangeFilletRadius = p.mTopFlangeFilletRadius;
			mBottomFlangeEdgeRadius = p.mBottomFlangeEdgeRadius;
			mBottomFlangeSlope = p.mBottomFlangeSlope;
			mTopFlangeEdgeRadius = p.mTopFlangeEdgeRadius;
			mTopFlangeSlope = p.mTopFlangeSlope;
			mCentreOfGravityInY = p.mCentreOfGravityInY;
		}
		public IfcAsymmetricIShapeProfileDef(DatabaseIfc db, string name, double bottomFlangeWidth, double overallDepth, double webThickness, double bottomFlangeThickness, double topFlangeWidth)
			: base(db, name)
		{
			BottomFlangeWidth = bottomFlangeWidth;
			OverallDepth = overallDepth;
			WebThickness = webThickness;
			BottomFlangeThickness = bottomFlangeThickness;
			TopFlangeWidth = topFlangeWidth;
		}
	}
	[Serializable]
	public partial class IfcAudioVisualAppliance : IfcFlowTerminal //IFC4
	{
		internal IfcAudioVisualApplianceTypeEnum mPredefinedType = IfcAudioVisualApplianceTypeEnum.NOTDEFINED;// OPTIONAL : IfcAudioVisualApplianceTypeEnum;
		public IfcAudioVisualApplianceTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAudioVisualAppliance() : base() { }
		internal IfcAudioVisualAppliance(DatabaseIfc db, IfcAudioVisualAppliance a, DuplicateOptions options) : base(db,a, options) { mPredefinedType = a.mPredefinedType; }
	}
	[Serializable]
	public partial class IfcAudioVisualApplianceType : IfcFlowTerminalType
	{
		internal IfcAudioVisualApplianceTypeEnum mPredefinedType = IfcAudioVisualApplianceTypeEnum.NOTDEFINED;// : IfcAudioVisualApplianceBoxTypeEnum; 
		public IfcAudioVisualApplianceTypeEnum PredefinedType { get { return mPredefinedType; } set { mPredefinedType = value; } }

		internal IfcAudioVisualApplianceType() : base() { }
		internal IfcAudioVisualApplianceType(DatabaseIfc db, IfcAudioVisualApplianceType t, DuplicateOptions options) : base(db, t, options) { mPredefinedType = t.mPredefinedType; }
		public IfcAudioVisualApplianceType(DatabaseIfc m, string name, IfcAudioVisualApplianceTypeEnum t) : base(m) { Name = name; mPredefinedType = t; }
	}
	[Serializable]
	public partial class IfcAxis1Placement : IfcPlacement
	{
		private int mAxis;//  : OPTIONAL IfcDirection
		public IfcDirection Axis { get { return (mAxis > 0 ? mDatabase[mAxis] as IfcDirection : null); } set { mAxis = (value == null ? 0 : value.mIndex); } }
		
		internal IfcAxis1Placement() : base() { }
		internal IfcAxis1Placement(DatabaseIfc db, IfcAxis1Placement p, DuplicateOptions options) : base(db, p, options) { if(p.mAxis > 0) Axis = db.Factory.Duplicate( p.Axis) as IfcDirection; }
		public IfcAxis1Placement(DatabaseIfc db) : base(db) { }
		public IfcAxis1Placement(IfcCartesianPoint location) : base(location) { }
		public IfcAxis1Placement(IfcDirection axis) : base(axis.mDatabase) { Axis = axis; }
		public IfcAxis1Placement(IfcCartesianPoint location, IfcDirection axis) : base(location) { Axis = axis; }
	}
	public partial interface IfcAxis2Placement : IBaseClassIfc { bool IsXYPlane { get; } } //SELECT ( IfcAxis2Placement2D, IfcAxis2Placement3D);
	[Serializable]
	public partial class IfcAxis2Placement2D : IfcPlacement, IfcAxis2Placement
	{ 
		private IfcDirection mRefDirection;// : OPTIONAL IfcDirection;
		public IfcDirection RefDirection { get { return mRefDirection; } set { mRefDirection = value; } }
		
		internal IfcAxis2Placement2D() : base() { }
		internal IfcAxis2Placement2D(DatabaseIfc db, IfcAxis2Placement2D p, DuplicateOptions options) : base(db, p, options)
		{
			if (p.mRefDirection != null)
				RefDirection = db.Factory.Duplicate(p.RefDirection) as IfcDirection;
		}
		public IfcAxis2Placement2D(DatabaseIfc db) : base(db.Factory.Origin2d) { }
		public IfcAxis2Placement2D(IfcCartesianPoint location) : base(location) { }
		
		public override bool IsXYPlane { get { return base.IsXYPlane && (mRefDirection == null || RefDirection.isXAxis); } }
	}
	[Serializable]
	public partial class IfcAxis2Placement3D : IfcPlacement, IfcAxis2Placement
	{
		private IfcDirection mAxis = null;// : OPTIONAL IfcDirection;
		private IfcDirection mRefDirection = null;// : OPTIONAL IfcDirection; 

		public IfcDirection Axis
		{
			get { return mAxis; }
			set
			{
				mAxis = value;
				if (value != null)
				{
					if (mRefDirection == null && mDatabase != null)
						RefDirection = (Math.Abs(value.DirectionRatioX - 1) < 1e-3 ? mDatabase.Factory.YAxis : mDatabase.Factory.XAxis);
				}
			}
		}
		public IfcDirection RefDirection
		{
			get { return mRefDirection; }
			set
			{
				mRefDirection = value;
				if (value != null)
				{
					if (mAxis == null && mDatabase != null)
						Axis = (Math.Abs(value.DirectionRatioZ - 1) < 1e-3 ? mDatabase.Factory.XAxis : mDatabase.Factory.ZAxis);
				}
			}
		}

		internal IfcAxis2Placement3D() : base() { }
		internal IfcAxis2Placement3D(DatabaseIfc db, IfcAxis2Placement3D p, DuplicateOptions options) : base(db, p, options)
		{
			if (p.mAxis != null)
				Axis = db.Factory.Duplicate(p.Axis) as IfcDirection;
			if (p.mRefDirection != null)
				RefDirection = db.Factory.Duplicate(p.RefDirection) as IfcDirection;
		}
		public IfcAxis2Placement3D(IfcCartesianPoint location) : base(location) { }
		public IfcAxis2Placement3D(IfcCartesianPoint location, IfcDirection axis, IfcDirection refDirection) : base(location) { Axis = axis; RefDirection = refDirection; }
		
		public override bool IsXYPlane
		{
			get
			{
				if (mAxis != null && !Axis.isZAxis)
					return false;
				if (mRefDirection != null && !RefDirection.isXAxis)
					return false;
				return base.IsXYPlane;
			}
		}
	}
	[Serializable]
	public abstract partial class IfcAxisLateralInclination : IfcGeometricRepresentationItem
	{
		//INVERSE
		private SET<IfcLinearAxisWithInclination> mToLinearAxis = new SET<IfcLinearAxisWithInclination>();
		//INVERSE
		public SET<IfcLinearAxisWithInclination> ToLinearAxis { get { return mToLinearAxis; } set { mToLinearAxis = value; } }

		protected IfcAxisLateralInclination() : base() { }
		protected IfcAxisLateralInclination(DatabaseIfc db) : base(db) { }
		protected IfcAxisLateralInclination(DatabaseIfc db, IfcAxisLateralInclination axisLateralInclination, DuplicateOptions options) : base(db, axisLateralInclination, options) { }
	}
}
