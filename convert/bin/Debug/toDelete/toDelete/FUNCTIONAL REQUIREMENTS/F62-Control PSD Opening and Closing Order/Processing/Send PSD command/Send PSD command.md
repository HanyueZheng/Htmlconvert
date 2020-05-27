
[iTC_CC_ATP-SwRS-0336]
PSDopeningCommand，本ATP或者冗余端ATP当前是否在发送开PSD命令.
ATP shall determine whether itself or the redundant ATP opening the PSD in this cycle.
```
	if ((PSDplatformManagerOpeningOrder(k) == True)
	    or (OtherATP.PsdManagerOpeningOrder(k) == True))
	    PSDopeningCommand = True
	else:
	    PSDopeningCommand = False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0230]
[End]
[iTC_CC_ATP-SwRS-0444]
在与联锁通信时，如果PSDmanagerOrder_A.Id有效，则根据PSDmanagerOrder_A.Order和离线配置数据设置发送给A侧屏蔽门的控制信息CIsetting[0]。
When communicating with the CI, if the PSDmanagerOrder_A which comes from CCNV was valid, ATP shall set the CIsetting[0] according to the PSDmanagerOrder_A and the configuration of the PSD.
```
	if ((CommunicateWithPSD(k) == True)
	     and (PSDmanagerOrder_A.Id != None))
	    CIsetting[0].PlatformId = PSDmanagerOrder_A.Id
	    if (PSDmanagerOrder_A.Order == Open_PSD_Configuration)
	        CIsetting[0].Order = TrackMap.PSDs[PSDmanagerOrder_A.Id].DoorOpeningCode
	    elif (PSDmanagerOrder_A.Order == Close_PSD_Configuration):
	        CIsetting[0].Order = TrackMap.PSDs[PSDmanagerOrder_A.Id].DoorClosingCode
	    else:
	        CIsetting[0].Order = TrackMap.PSDs[PSDmanagerOrder_A.Id].DoorNoActionCode
	else:
	    CIsetting[0].PlatformId = None
	    CIsetting[0].Order = None
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0265]
[End]
[iTC_CC_ATP-SwRS-0445]
在与联锁通信时，如果PSDmanagerOrder_B.Id有效，则根据PSDmanagerOrder_B.Order和离线配置数据设置发送给B侧屏蔽门的控制信息CIsetting [1]。
When communicating with the CI, if the PSDmanagerOrder_B, which comes from CCNV, was valid, ATP shall set the CIsetting[1] according to the PSDmanagerOrder_B and the configuration of the PSD.
```
	if ((CommunicateWithPSD(k) == True)
	     and (PSDmanagerOrder_B.Id != None)):
	    CIsetting[1].PlatformId = PSDmanagerOrder_B.Id
	    if (PSDmanagerOrder_B.Order == Open_PSD_Configuration)
	        CIsetting[1].Order = TrackMap.PSDs[PSDmanagerOrder_B.Id].DoorOpeningCode
	    elif (PSDmanagerOrder_B.Order == Close_PSD_Configuration):
	        CIsetting[1].Order = TrackMap.PSDs[PSDmanagerOrder_B.Id].DoorClosingCode
	    else:
	        CIsetting[1].Order = TrackMap.PSDs[PSDmanagerOrder_B.Id].DoorNoActionCode
	else:
	    CIsetting[1].PlatformId = None
	    CIsetting[1].Order = None
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0265]
[End]
