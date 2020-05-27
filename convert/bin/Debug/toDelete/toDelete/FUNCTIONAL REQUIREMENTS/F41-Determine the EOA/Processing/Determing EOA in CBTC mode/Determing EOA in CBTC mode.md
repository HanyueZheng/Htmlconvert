
[iTC_CC_ATP-SwRS-0669]
TrainInSMIzone，判断当车头最大定位在SMI区域内，且车速小于SMI限速时，可使用ZC的EOA消息中的WithoutSpacingEoa进行监控。
```
	def TrainInSMIzone(k):
	    Smi = TrackMap.ExistZoneLocationIncluded(SGL_SMI_ZONE, TrainFrontEnd(k).Max)
	    return (Smi is not None
	            and TrainMaxSpeed(k) < Smi.SpeedLimit(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1281], [iTC_CC-SyAD-1282], [iTC_CC-SyAD-1283]
[End]
[iTC_CC_ATP-SwRS-0160]
CBTCmodeEOAvalid，CBTC模式下判断来自ZC的EOA是否有效。
如果在SMI区域内且车速小于SMI限速，则应当使用WithoutSpacingEOA；
否则，应当使用普通的EOA
```
	def CBTCmodeEOAvalid(k):
	    return (not BlockModeUsed(k)
	            and ReceivedEOAreport.TrainFrontEnd == TrainFrontEnd(k)
	            and ((TrainInSMIzone(k)
	                  and (Message.IsMoreRecent(ReceivedEOAreport(k).WithoutSpacing.ValidityTime,
	                                                 ATPtime(k)))
	                  and (ReceivedEOAreport(k).WithoutSpacing.Location.Block != 0)
	                  and (TrackMap.DistanceBtwTwoLocs(TrainFrontLocation(k).Min,
	                                                         ReceivedEOAreport(k).WithoutSpacing.Location,
	                                                         ATPsetting.EOAmaxDistance) is not None))
	                 or (not TrainInSMIzone(k)
	                     and TrainLocatedOnKnownPath(k)
	                     and (Message.IsMoreRecent(ReceivedEOAreport(k).Classic.ValidityTime,
	                                                    ATPtime(k)))
	                     and (ReceivedEOAreport(k).Classic.Location.BlockId != 0)
	                     and (TrackMap.DistanceBtwTwoLocs(TrainFrontLocation(k).Min,
	                                                             ReceivedEOAreport(k).Eoa.Location,
	                                                             ATPsetting.EOAmaxDistance) is not None))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0154], [iTC_CC-SyAD-0157], [iTC_CC-SyAD-0287], [iTC_CC-SyAD-0288], [iTC_CC-SyAD-0289], [iTC_CC-SyAD-0293], [iTC_CC-SyAD-0299], [iTC_CC-SyAD-0842], [iTC_CC-SyAD-1282], [iTC_CC_ATP_SwHA-0028], [iTC_CC_ATP_SwHA-0047], [iTC_CC_ATP_SwHA-0052], [iTC_CC_ATP_SwHA-0252]
[End]
NOTES:
对于普通EOA，ZC会检查发送的EOA坐标，确保其在Block长度范围内。而对于布置了SMI区的项目，为使得列车能尽量靠近轨道末端的车档停车，WithoutSpacing类型的EOA坐标可能为负值，或者大于所在Block长度，而其所在BlockID仍为轨道末端的Block。
[iTC_CC_ATP-SwRS-0670]
CBTCmodeEOAlocation，CBTC下的EOA位置。
```
	def CBTCmodeEOAlocation(k):
	    if (CBTCmodeEOAvalid(k)):
	        if (TrainInSMIzone(k)):
	            return ReceivedEOAreport.WithoutSpacing.Location
	        else:
	            return ReceivedEOAreport.Classic.Location
	    else:
	        return None
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1282], [iTC_CC-SyAD-1289], [iTC_CC_ATP_SwHA-0252]
[End]
