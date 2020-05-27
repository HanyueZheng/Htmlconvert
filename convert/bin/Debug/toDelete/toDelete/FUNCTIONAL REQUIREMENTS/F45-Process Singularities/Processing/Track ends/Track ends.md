﻿
开放轨道终点：
当位于车头最大定位和EB实际施加位置之间时，ATP需将其作为安全速度限制区域处理，其限速值为0；
当位于EBA点下游时，作为安全限制点，限速为0。
[iTC_CC_ATP-SwRS-0697]
ZoneVSLnotExceedOTE，Open track end作为区域型限速的情形
```
	def ZoneVSLnotExceedOTE(k):
	    if (TrackMap.ExistSingularityInZone(SGL_OPEN_TRACK_END, TrainFrontLocation(k).Max,
	                                               X2EbApplied(k)) is not None)):
	        return False
	    else: 
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0300], [iTC_CC-SyAD-0304], [iTC_CC_ATP_SwHA-0258]
[End]
[iTC_CC_ATP-SwRS-0698]
PointVSLnotExceedOTE，Open track end作为点型限速的情形
```
	def PointVSLnotExceedOTE(k):
	     Ote = (TrackMap.ExistSingularityInZone(SGL_OPEN_TRACK_END,
	                                TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                                ATPsetting.EOAmaxDistance)
	    if (Ote is not None)
	        and TrainEnergy(k) >= (Energy.AccumulationPotentialEnergy
	                                   (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                       X2EbApplied(k)),
	                                    Ote.Location))):
	        return False
	    else:
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0300], [iTC_CC-SyAD-0304], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC_ATP_SwHA-0258]
[End]
封闭轨道终点：
当位于车头最大定位和EB实际施加位置之间时，ATP需将其作为安全速度限制区域处理，其限速值为该奇点的属性值.；
当位于EBA下游时，作为安全限速点，其限速为线路地图中描述的值。
[iTC_CC_ATP-SwRS-0699]
ZoneVSLnotExceedCTE，Close track end作为区域型限速的情形
```
	def ZoneVSLnotExceedCTE(k):
	    cte = (TrackMap.ExistSingularityInZone(SGL_CLOSE_TRACK_END, TrainFrontLocation(k).Max,
	                                                   X2EbApplied(k)))
	    if (cte is not None
	        and TrainEnergy(k) >= pow(cte.SpeedLimit)):
	        return False
	    else: 
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0300], [iTC_CC-SyAD-1033], [iTC_CC_ATP_SwHA-0258]
[End]
[iTC_CC_ATP-SwRS-0700]
PointVSLnotExceedCTE，Close track end作为点型限速的情形
```
	def PointVSLnotExceedCTE(k):
	    cte = (TrackMap.ExistSingularityInZone(SGL_CLOSE_TRACK_END,
	                                TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                                ATPsetting.EOAmaxDistance))
	    if (cte is not None
	        and TrainEnergy(k) >= (pow(cte.SpeedLimit)
	                                   + (Energy.AccumulationPotentialEnergy
	                                       (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                           X2EbApplied(k)),
	                                      cte.Location)))):
	        return False
	    else:
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC-SyAD-1033], [iTC_CC_ATP_SwHA-0258]
[End]