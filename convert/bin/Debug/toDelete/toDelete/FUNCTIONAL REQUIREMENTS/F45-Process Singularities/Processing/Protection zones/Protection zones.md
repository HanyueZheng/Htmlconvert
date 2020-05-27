
保护区：
如果从列车车尾最小定位到EB实际施加位置的范围内存在限制状态的保护区，ATP需将其作为安全速度限制区域处理，其限速值为0；
如果在EBA下游存在限制状态的保护区起始点，其限速为0。
[iTC_CC_ATP-SwRS-0707]
ZoneVSLnotExceedPZ，PZ作为区域型限速。ATP应监控与列车定位有以下两种关系的限制状态保护区：
该保护区的起始点在车尾最小定位到紧急制动施加位置之间；
或，该保护区起始点在车尾最小定位上游，但车尾最小定位在该保护区范围内。
```
	def ZoneVSLnotExceedPZ(k):
	    for Pz in (TrackMap.AllSingsInZone(SGL_PROTECTION_ZONE,
	                                              TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                             TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)))):
	        if ((not TrackMap.LocationBtwTwoLocs(Pz.Location,
	                                                    TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                                                    TrainRearLocation(k).Min)
	              or TrackMap.LocationInZone(TrainRearLocation(k).Min, Pz.Location, Pz.Length))
	            and not CoercedPermissive(Pz.CoercedPermissive, k)
	            and (CoercedRestrictive(Pz.NotCoercedRestrictive, k)
	                 or not VariantValue(Pz.Variant, k))):
	            return False
	        else:
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0225], [iTC_CC-SyAD-0294], [iTC_CC-SyAD-0295], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0796], [iTC_CC-SyAD-1273], [iTC_CC-SyAD-1274], [iTC_CC-SyAD-1277], [iTC_CC_ATP_SwHA-0258]
[End]
[iTC_CC_ATP-SwRS-0708]
PointVSLnotExceedPZ，PZ作为点型限速的情形
```
	def PointVSLnotExceedPZ(k):
	    for Pz in (TrackMap.AllSingsInZone(SGL_PROTECTION_ZONE, 
	                               TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                               ATPsetting.EOAmaxDistance)):
	        if (not CoercedPermissive(Pz.CoercedPermissive, k)
	            and (CoercedRestrictive(Pz.NotCoercedRestrictive, k)
	                 or not VariantValue(Pz.Variant, k))
	            and TrainEnergy(k) >= (Energy.AccumulationPotentialEnergy
	                                      (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                          X2EbApplied(k)),
	                                       Pz.Location))):
	            return False
	        else:
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0225], [iTC_CC-SyAD-0294], [iTC_CC-SyAD-0295], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC-SyAD-0796], [iTC_CC-SyAD-1273], [iTC_CC-SyAD-1274], [iTC_CC-SyAD-1277], [iTC_CC_ATP_SwHA-0258]
[End]
