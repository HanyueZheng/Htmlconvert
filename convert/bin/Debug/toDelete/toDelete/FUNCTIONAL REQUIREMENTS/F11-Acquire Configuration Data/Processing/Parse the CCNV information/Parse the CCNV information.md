﻿
[iTC_CC_ATP-SwRS-0125]
NonVitalRequestReady，通过与CCNV的通信接口，判断是否收到CCNV的消息NonVitalRequest
Through the communication with CCNV, ATP judges NonVitalRequestreceived from CCNV and generates NonVitalRequestReady If received a new message.
```
	def NonVitalRequestReady(k):
	    return Message.Exists(NonVitalRequest)
```
\#Category= Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0068]
[End]
[iTC_CC_ATP-SwRS-0126]
ATOcontrolTimeValid，CCNV消息有效标志，如果超过CCNV_VALIDITY_CYCLES个周期仍未收到新的CCNV消息，则设置为False。
ATOcontrolTimeValid stands for the effectiveness of CCNV message. If there is no updating CCNV message past the CCNV_VALIDITY_CYCLES, ATOcontrolTimeValid is set as False.
```
	def ATOcontrolTimeValid(k):
	    if (NonVitalRequestReady(k)):
	        ATOcontrolTimeValid = True
	        ATOcontrolTimer = 0
	    elif (ATOcontrolTimer(k-1) < CCNV_VALIDITY_CYCLES):
	        ATOcontrolTimer = ATOcontrolTimer(k-1) + 1
	    else:
	        ATOcontrolTimeValid = False
	    return ATOcontrolTimeValid
```
\#Category= Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1044]
[End]