
临时限速：
如果从列车车尾最小定位到EB实际施加位置的范围内存在临时限速，需作为安全速度限制区域，其限速值为从LC收到TSR消息中相应的速度值；
如果在EBA点下游存在限速减小的TSR，则ATP将其起始点作为安全限速点，其限速值为从LC收到TSR消息中相应的速度值。
[iTC_CC_ATP-SwRS-0069]
TSRcontrolInhibition，不处理TSR信息。其状态来自于项目可配置的列车输入采集。
According to the status of TSRcontrollinhibition, ATP can judge whether it is necessary to handle TSR information.
```
	def TSRcontrolInhibition(k):
	    return Offline.GetTSRcontrolInhibition(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0281], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-1003], [iTC_CC-SyAD-1310], [iTC_CC_ATP_SwHA-0205], [iTC_CC_ATP_SwHA-0261]
[End]
[iTC_CC_ATP-SwRS-0695]
ZoneVSLnotExceedTSR，TSR作为区域型限速的情形。即对于从车尾所在Block起始点到EB施加位置内的所有Block，当满足以下条件时，认为列车超过了TSR限速：
未禁止处理TSR信息；
且该Block存在TSR；
且列车定位与该TSR区域有交集；
且计算的列车能量大于上述TSR的限制能量。
```
	def ZoneVSLnotExceedTSR(k):
	    for Blk in (TrackMap.AllSingsBtwTwoLocs(SGL_NEW_BLOCK,
	                              TrackMap.BlockOrigin(TrainRearLocation(k).Min)
	                              TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)))):
	        Tsr = TSRonBlock(Blk, TrackMap.OppositeOrientation(TrainFrontOrientation(k)), k)
	        if (not TSRcontrolInhibition(k)
	            and Tsr is not None
	            and not TrackMap.LocationBtwTwoLocs(Tsr.Position,
	                                                       TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                                                       TrainRearLocation(k).Min)
	            and TrainEnergy(k) >= pow(Tsr.Value)):
	            return False
	        else:
	            continue
	    return True
```
其中TSRonBlock表示获取指定Block上TSR的值。
```
	def TSRonBlock(blockId, direction, k):
	    if (not ReceivedTSRdatabase.Blocks[blockId].NotRestrictionApplication):
	        if (direction is UP):
	            return (ReceivedTSRdatabase.Blocks[blockId].Position[0],
	                      ReceivedTSRdatabase.Blocks[blockId].Value)
	        else:
	            return (ReceivedTSRdatabase.Blocks[blockId].Position[1],
	                      ReceivedTSRdatabase.Blocks[blockId].Value)
	    else:
	        return None
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0285], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-1310], [iTC_CC_ATP_SwHA-0261]
[End]
[iTC_CC_ATP-SwRS-0696]
PointVSLnotExceedTSR，TSR作为点型限速的情形
```
	def PointVSLnotExceedTSR(k):
	    for Blk in (TrackMap.AllSingsInZone(SGL_NEW_BLOCK,
	                                TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                                ATPsetting.EOAmaxDistance)):
	        Tsr = TSRonBlock(Blk, TrainFrontOrientation(k), k)
	        if (not TSRcontrolInhibition(k)
	            and Tsr is not None
	            and TrainEnergy(k) >= (pow(Tsr.Value)
	                                       + (Energy
	.AccumulationPotentialEnergy                                           (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                               X2EbApplied(k)),
	                                           Tsr.Position)))):
	            return False
	        else:
	            continue
	    else:
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0285], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC-SyAD-1310], [iTC_CC_ATP_SwHA-0261]
[End]
