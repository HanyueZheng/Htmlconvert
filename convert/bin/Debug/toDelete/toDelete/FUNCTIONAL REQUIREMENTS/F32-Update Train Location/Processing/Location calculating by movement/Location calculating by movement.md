﻿
[iTC_CC_ATP-SwRS-0259]
LocationBeforeReloc，上周期列车已定位的情况下，使用里程计测得的位移来更新列车定位。
If train has localized on the track map, according to the orientation of END_2, ATP using the maximum and minimum train motion to update the external or internal location of the END_2.
```
	def LocationBeforeReloc(k):
	    if (TrainLocalized(k-1) and ValidTrainKinematic(k)):
	        if (OdometerState(k) is INITIALIZED):
	            if (End2RunningForward(k)):
	                LocationBeforeReloc.Uncertainty = (TrainLocation(k-1).Uncertainty
	                                                     - (MaximumTrainMotion(k) - MinimumTrainMotion(k)))
	            else:
	                LocationBeforeReloc.Uncertainty = (TrainLocation(k-1).Uncertainty
	                                                     + (MaximumTrainMotion(k) - MinimumTrainMotion(k)))
	        else:
	            LocationBeforeReloc.Uncertainty = (TrainLocation(k-1).Uncertainty
	                                                      + abs(MaximumTrainMotion(k))
	                                                      + abs(MinimumTrainMotion(k))) 
	        LocationBeforeReloc.Ext2 = (TrackMap.LocationUpdateExt2(End2RunningForward(k),
	                                                                           TrainLocation(k-1).Ext2.Ort,
	                                                                           TrainLocation(k-1).Ext2,
	                                                                           MaximumTrainMotion(k),
	                                                                           MinimumTrainMotion(k)))
	        LocationBeforeReloc.Int2 = U
	pdateInt2FromExt2        LocationBeforeReloc.Ext1 = U
	pdateExt1FromExt2        LocationBeforeReloc.Int1 = U
	pdateInt1FromExt2    else:
	        LocationBeforeReloc = None
	    return LocationBeforeReloc
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0172], [iTC_CC-SyAD-0184], [iTC_CC_ATP_SwHA-0096]
[End]
