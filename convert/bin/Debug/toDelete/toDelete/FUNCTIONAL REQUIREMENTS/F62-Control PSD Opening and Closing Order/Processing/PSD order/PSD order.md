﻿
对于PSD的开关门指令，ATP根据来自CCNV的指令，根据项目配置的安全通信协议进行发送。
For the control order of the PSD, ATP shall generate according to the request of the CCNV and the safety communication protocol defined in the project configuration.
[iTC_CC_ATP-SwRS-0333]
PSDmanagerOrder_A，A侧PSD的控制命令信息，其结构为ST_PSD_MANAGE。其中如果来自CCNV的A侧PSD标识不等于ATP读取SGD中A侧的标识，则禁止使用CCNV的标识开门。
The rules to generate the PSD manage order on side A shall follow the pseudo-codes. In which if the PSD id from CCNV is not equal to the id in ATP's track map, ATP shall prohibit the PSD opening.
```
	def PSDmanagerOrder_A(k):
	    PSDmanagerOrder_A.Id = PSDoperation_A.Id(k)
	    if (PSDoperation_A.Id(k) == PSDid_A(k)
	        and PSDoperation_A.Id(k) is not None
	        and not PSDoperation_A.ClosingOrder(k)
	        and PSDoperation_A.OpeningOrder(k)
	        and EnableDoorOpening_A(k)):
	        PSDmanagerOrder_A.Order = Open_PSD_Configuration
	    elif (not PSDoperation_A.OpeningOrder(k)
	           and PSDoperation_A.ClosingOrder(k)):
	        PSDmanagerOrder_A.Order = Close_PSD_Of_Platform
	    else:
	        PSDmanagerOrder_A.Order = None
	    return PSDmanagerOrder_A 
```
In the above ARDL, the Open_PSD_Configuration and Close_PSD_Of_Platform are control words defined in the project configuration.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0265], [iTC_CC_ATP_SwHA-0139]
[End]
[iTC_CC_ATP-SwRS-0334]
PSDmanagerOrder_B，B侧PSD的控制命令信息，其结构为ST_PSD_MANAGE。其中如果来自CCNV的B侧PSD标识不等于ATP读取SGD中B侧的标识，则禁止使用来自CCNV的标识开门。
The rules to generate the PSD manage order on side B shall follow the pseudo-codes. In which if the PSD id from CCNV is not equal to the id in ATP's track map, ATP shall prohibit the PSD opening.
```
	def PSDmanagerOrder_B(k):
	    PSDmanagerOrder_B.Id = PSDoperation_B.Id(k)
	    if (PSDoperation_B.Id(k) == PSDid_B(k)
	        and PSDoperation_B.Id(k) is not None
	        and not PSDoperation_B.ClosingOrder(k)
	        and PSDoperation_B.OpeningOrder(k)
	        and EnableDoorOpening_B(k)):
	        PSDmanagerOrder_B.Order = Open_PSD_Configuration
	    elif (not PSDoperation_B.OpeningOrder(k)
	           and PSDoperation_B.ClosingOrder(k)):
	        PSDmanagerOrder_B.Order = Close_PSD_Of_Platform
	    else:
	        PSDmanagerOrder_B.Order = None
	    return PSDmanagerOrder_B 
```
In the above ARDL, the Open_PSD_Configuration and Close_PSD_Of_Platform are control words defined in the project configuration.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0265], [iTC_CC_ATP_SwHA-0139]
[End]
[iTC_CC_ATP-SwRS-0335]
PSDplatformManagerOpeningOrder，本ATP是否发了开门命令.
ATP shall determine whether itself opening the PSD in this cycle.
```
	if (Initialization)
	    PSDplatformManagerOpeningOrder = False
	elif ((PSDmanagerOrder_A.Order(k) == Open_PSD_Configuration)
	        or (PSDmanagerOrder_B.Order(k) == Open_PSD_Configuration))
	    PSDplatformManagerOpeningOrder = True
	else:
	    PSDplatformManagerOpeningOrder = False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0230]
[End]