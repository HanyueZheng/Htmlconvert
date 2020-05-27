
[iTC_CC_ATP-SwRS-0080]
OtherATP，解析并存储远端ATP的消息。
初始化或者远端消息过期时，设置相应的值为默认状态；
当本周期收到新的远端消息时，将其设置为新收到消息的值；
否则，保持不变
OtherATP, parse and store messages from the distant ATP.
In initialization or the message has expired, set all variables as default value;
when new message available, set the corresponding value from the new message;
otherwise, remain unchanged.
```
	def OtherATP(k):
	    if (Initialization
	        or (not OtherATPmessageValid(k))):
	        OtherATP.LatestTimeOtherCore = INVALID_LOOP_HOUR
	        OtherATP.CoreId = None
	        OtherATP.BeaconId = None
	        OtherATP.EnableDoorOpening_A = False
	        OtherATP.EnableDoorOpening_B = False
	        OtherATP.PsdManagerOpeningOrder = False
	        OtherATP.PsdIdSide_A = None
	        OtherATP.PsdValiditySide_A = None
	        OtherATP.PsdClosedSide_A = False
	        OtherATP.PsdIdSide_B = None
	        OtherATP.PsdValiditySide_B = None
	        OtherATP.PsdClosedSide_B = False
	        OtherATP.ZcVersion = None
	        OtherATP.LocatedOnKnownPath = False
	        OtherATP.LocatedWithMemLocation = False
	        OtherATP.Location.Ext2 = None
	        OtherATP.Location.Uncertainty = None
	        OtherATP.Location.Ext1 = None
	        OtherATP.SleepZoneId = None
	        OtherATP.SleepZoneVersion = None
	        OtherATP.MotionSinceLastReloc = None
	        OtherATP.MotionSinceMemLoc = None
	        OtherATP.TrainFilteredStopped = False
	        OtherATP.SafetyParameterVersion = None
	        OtherATP.SafetyApplicationVersion = None
	        OtherATP.CC_SSID = None
	        OtherATP.OverlapExpired = False
	    elif (OtherATPmessageAvailable(k)):
	        OtherATP.LatestTimeOtherCore = OtherCCsynchroReport.LatestTimeOtherCore(k)
	        OtherATP.CoreId = OtherCCsynchroReport.CoreId
	        OtherATP.BeaconId = OtherCCsynchroReport.BeaconId
	        OtherATP.EnableDoorOpening_A = OtherCCsynchroReport.EnableDoorOpening_A
	        OtherATP.EnableDoorOpening_B = OtherCCsynchroReport.EnableDoorOpening_B
	        OtherATP.PsdManagerOpeningOrder = OtherCCsynchroReport.PsdManagerOpeningOrder
	        OtherATP.PsdIdSide_A = OtherCCsynchroReport.PsdIdSide_A
	        OtherATP.PsdValiditySide_A = OtherCCsynchroReport.PsdValiditySide_A
	        OtherATP.PsdClosedSide_A = OtherCCsynchroReport.PsdClosedSide_A
	        OtherATP.PsdIdSide_B = OtherCCsynchroReport.PsdIdSide_B
	        OtherATP.PsdValiditySide_B = OtherCCsynchroReport.PsdValiditySide_B
	        OtherATP.PsdClosedSide_B = OtherCCsynchroReport.PsdClosedSide_B
	        OtherATP.ZcVersion = OtherCCsynchroReport.ZcVersion
	        OtherATP.LocatedOnKnownPath = OtherCCsynchroReport.LocatedOnKnownPath
	        OtherATP.LocatedWithMemLocation = OtherCCsynchroReport.LocatedWithMemLocation
	        OtherATP.Location.Ext2 = OtherCCsynchroReport.Location.Ext2
	        OtherATP.Location.Uncertainty = OtherCCsynchroReport.Location.Uncertainty
	        OtherATP.Location.Ext1 = OtherCCsynchroReport.Location.Ext1
	        OtherATP.SleepZoneId = OtherCCsynchroReport.SleepZoneId
	        OtherATP.SleepZoneVersion = OtherCCsynchroReport.SleepZoneVersion
	        OtherATP.MotionSinceLastReloc = OtherCCsynchroReport.MotionSinceLastReloc
	        OtherATP.MotionSinceMemLoc = OtherCCsynchroReport.MotionSinceMemLoc
	        OtherATP.TrainFilteredStopped = OtherCCsynchroReport.TrainFilteredStopped
	        OtherATP.SafetyParameterVersion = OtherCCsynchroReport.SafetyParameterVersion
	        OtherATP.SafetyApplicationVersion = OtherCCsynchroReport.SafetyApplicationVersion
	        OtherATP.CC_SSID = OtherCCsynchroReport.CC_SSID
	        OtherATP.OverlapExpired = OtherCCsynchroReport.OverlapExpired
	    else:
	        pass
	    return OtherATP
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source= [iTC_CC-SyAD-0963], [iTC_CC-SyAD-1212], [iTC_CC_ATP_SwHA-0014], [iTC_CC_ATP_SwHA-0013]
[End]
