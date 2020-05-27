﻿
[iTC_CC_ATP-SwRS-0731]
CCvariants，ATP发送给联锁的Overlap解锁信息。
ATP shall check the following conditions when sending overlap release to CBI:
Train front location is in overlap release zone,
and the other ATP's overlap timer has expired,
and ATP received variant request from CBI in this zone.
```
	def CCvariants(CbiId, k):
	    Orz = TrackMap.ExistZoneLocationIncluded(SGL_OVERLAP_RELEASE_ZONE,
	                                                     TrainFrontLocation(k).Max):
	    if (OverlapReleasable(k)
	        and OtherATP(k).OverlapExpired
	        and CCvariantRequestMsgReceived(CbiId, k)
	        and Orz is not None
	        and Orz.CbiId == CbiId
	        and CbiId == NonVitalRequest(k).VariantRequestCbiId):
	        for Index in range(0, MAX_CC_VARIANTS_NB):
	            if (Orz.RadioBlockModeVariantIndex == Index):
	                CCvariants[CbiId].Variant[Index] = True
	            else:
	                CCvariants[CbiId].Variant[Index] = False
	    else:
	        CCvariants[CbiId] = None
	    return CCvariants
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1194], [iTC_CC-SyAD-1195], [iTC_CC-SyAD-1292], [iTC_CC_ATP_SwHA-0273]
[End]