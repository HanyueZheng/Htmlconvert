﻿
道岔：
如果在车头最大定位和EB实际施加位置之间存在非受控状态的道岔（无论汇聚节点还是发散节点），ATP需将其作为安全速度限制区域处理，其限速值为0. 
如果在EBA点下游存在非受控状态的道岔，ATP需将其作为安全速度限制点，其限速值为0；
[iTC_CC_ATP-SwRS-0705]
ZoneVSLnotExceedSwitch，非受控道岔作为区域型限速的情形
```
	def ZoneVSLnotExceedSwitch(k):
	    for Switch in TrackMap.AllSwitchesInZone(TrainFrontLocation(k).Max, X2EbApplied(k)):
	        if (VariantValue(Switch.Variant1, k) == VariantValue(Switch.Variant2, k)):
	            return False
	        else: 
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0294], [iTC_CC-SyAD-0295], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0868], [iTC_CC_ATP_SwHA-0258]
[End]
[iTC_CC_ATP-SwRS-0706]
PointVSLnotExceedSwitch，非受控道岔作为点型限速的情形
```
	def PointVSLnotExceedSwitch(k):
	    for Switch in (TrackMap
	.AllSwitchesInZone                      (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                       ATPsetting.EOAmaxDistance)):
	        if (VariantValue(Switch.Variant1, k) == VariantValue(Switch.Variant2, k)
	            and TrainEnergy(k) >= (Energy.AccumulationPotentialEnergy
	                                      (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                          X2EbApplied(k)),
	                                       Switch.Location))):
	            return False
	        else: 
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0294], [iTC_CC-SyAD-0295], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC-SyAD-0868], [iTC_CC_ATP_SwHA-0258]
[End]
