
[iTC_CC_ATP-SwRS-0056]
初始化时，ATP软件需将来自DataPlugContent的部分内容，如Table 47所示，在与CCNV建立通信后通过双口RAM发送给CCNV。如果ATP软件无法与CCNV建立通信，则保持等待，由CPU1在LED上显示WAITING _CCNV，直到通信建立成功或者操作人员重启VLE-2板。
In initialization, ATP needs to send some part of contents as shown in Table 47 from CC data plug to CCNV by dual-ports RAM after getting contact with CCNV. If ATP cannot establish the communication with CCNV, it will keep waiting and show in the LED as WAITING _CCNV, until the communication is built or the operator reboots VLE-2 board.
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source= [iTC_CC-SyAD-0067], [iTC_CC-SyAD-0831], [iTC_CC-SyAD-0032], [iTC_CC_VLE-2-DVCOM-2-SyID-0034], [iTC_CC_VLE-2-DVCOM-2-SyID-0071], [iTC_CC_VLE-2-DVCOM-2-SyID-0018], [iTC_CC-SyAD-
1423][End]
