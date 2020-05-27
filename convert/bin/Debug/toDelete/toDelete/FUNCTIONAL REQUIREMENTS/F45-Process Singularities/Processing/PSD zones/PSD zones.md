
屏蔽门区域：
如果从列车车尾最小定位到EB实际施加位置的范围内存在限制状态的PSD，ATP需将其作为安全速度限制区域处理，其限速值为0；
如果EBA下游存在限制状态的屏蔽门区起始点，其限速为0。
[iTC_CC_ATP-SwRS-0709]
ZoneVSLnotExceedPSD，PSD作为区域型限速。ATP应监控与列车定位有以下两种关系的限制状态屏蔽门区域：
该屏蔽门区的起始点在车尾最小定位到紧急制动施加位置之间；
或，该屏蔽门区起始点在车尾最小定位上游，但车尾最小定位在该屏蔽门区范围之内。
```
	def ZoneVSLnotExceedPSD(k):
	    for Psd in (TrackMap.AllSingsBtwTwoLocs(SGL_PSD_ZONE,
	                                                   TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                              TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)))):
	        if ((not TrackMap.LocationBtwTwoLocs(Psd.Location,
	                                                    TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                                                    TrainRearLocation(k).Min)
	              or TrackMap.LocationInZone(TrainRearLocation(k).Min, Psd.Location, Psd.Length))
	            and not CoercedPermissive(Psd.CoercedPermissive, k)
	            and (CoercedRestrictive(Psd.NotCoercedRestrictive, k)
	                 or not VariantValue(Psd.Variant, k))):
	            return False
	        else:
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0294], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-1398], [iTC_CC-SyAD-1399], [iTC_CC_ATP_SwHA-0258]
[End]
[iTC_CC_ATP-SwRS-0710]
PointVSLnotExceedPSD，PSD作为点型限速的情形
```
	def PointVSLnotExceedPSD(k):
	    for Psd in (TrackMap.AllSingsInZone(SGL_PSD_ZONE,
	                              TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k) ),
	                              ATPsetting.EOAmaxDistance)):
	        if (not CoercedPermissive(Psd.CoercedPermissive, k)
	            and (CoercedRestrictive(Psd.NotCoercedRestrictive, k)
	                 or not VariantValue(Psd.Variant, k))
	            and TrainEnergy(k) >= (Energy.AccumulationPotentialEnergy
	                                      (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                          X2EbApplied(k)),
	                                       Psd.Location))):
	            return False
	        else:
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0294], [iTC_CC-SyAD-0301], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC-SyAD-1398], [iTC_CC-SyAD-1399], [iTC_CC_ATP_SwHA-0258]
[End]
