
当ZC检测到计轴故障时，会自动激活该故障计轴所在的Block上的限速。ATP应检测车身范围及其下游Block的BSR变量，如果存在该变量且为限制状态，则认为该Block上的BSR激活，ATP应确保列车速度低于该限速。
[iTC_CC_ATP-SwRS-0693]
ZoneVSLnotExceedBSR，车身范围内有BSR的情形
```
	def ZoneVSLnotExceedBSR(k):
	    for Block in (TrackMap.AllSingsBtwTwoLocs(SGL_NEW_BLOCK,
	                             TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                             TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)))):
	        if (Block.Bsr is not None
	            and not CoercedPermissive(Block.CoercedPermissive, k)
	            and not VariantValue(Block.Bsr.Variant, k)
	            and TrainEnergy(k) >= pow(Block.Bsr.Speed)):
	            return False
	        else:
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0300], [iTC_CC-SyAD-1267], [iTC_CC-SyAD-1268], [iTC_CC_ATP_SwHA-0261]
[End]
[iTC_CC_ATP-SwRS-0694]
PointVSLnotExceedBSR，列车下游有BSR的情形
```
	def PointVSLnotExceedBSR(k):
	    for Block in (TrackMap.AllSingsInZone(SGL_NEW_BLOCK,
	                               TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                               ATPsetting.EOAmaxDistance)):
	        if (Block.Bsr is not None
	            and not CoercedPermissive(Block.Bsr.CoercedPermissive, k)
	            and not VariantValue(Block.Bsr.Variant, k)
	            and TrainEnergy(k) >= (pow(Block.Bsr.Speed)
	                                       + (Energy.AccumulationPotentialEnergy
	                                          (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                              X2EbApplied(k)),
	                                           Block.Bsr.Position)))):
	            return False 
	        else:
	            continue
	    else:
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC-SyAD-1267], [iTC_CC-SyAD-1268], [iTC_CC_ATP_SwHA-0261]
[End]
