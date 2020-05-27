
ATP软件每周期在主任务中，根据上周期各中断锁存的编码里程计信息，对里程计所在车轮的动力学参数进行计算。即根据寄存器读值，判断本周期转过的齿数；如果未检测到里程计转动，则需进行传感器测试的判断，并据此检测车轮是否完全静止。根据编码里程计的设计，在传感器测试中不可能发生三路全导通或者堵塞的状态。
In every cycle of main task, according to coded odometer information of each interrupt latch in the last cycle, ATP software shall calculate the kinematic parameters of the wheel. That is to say, ATP shall judge the cog numbers in this cycle according to the value in the register. If ATP did not detect the move of odometer, it will check whether the wheel is completely static based on the sensor testing. According to the design of coded odometer, it is impossible that three sensors are in conduct or blocked at the same time.
[iTC_CC_ATP-SwRS-0592]
CompCogCode，ATP软件需要根据编码里程计的码盘特性和旋转方向，计算8个比特的期望齿号值。
当里程计初始化成功时，设置CompCogCode为初始的CogCode。
此后，对中断中转过的每个齿：
如果相邻中断齿数递增，期望齿号由高位向低位右移1个比特，将新的比特C4array[C4ArrayIndex]放在最高位，并更新C4ArrayIndex。
如果相邻中断齿数递减，期望齿号由低位向高位左移1个比特，将新的比特C4array[C4ArrayIndex]放在最低位，并更新C4ArrayIndex。
其中，C4ArrayIndex为当前对应的齿数索引，取值为0~99。C4array[C4ArrayIndex]为当前齿数对应码盘的通堵状态，1表示导通，0表示堵住，详见[REF4]。
The ATP software needs to calculate the expected cog code with 8 bits, according to the encoding characteristic of the disc code and the direction of odometer rotation.
When the odometer initialization, the expected CompCogCode shall be set as initial CogCode;
Since then, for one cog rotated in interrupt, the corresponded bit shall be shift as following rules:
if the cog increased in adjacent interrupts, the CompCogCode shall be shift a bit toward right from high to low; shift out the lowest one and set the new highest bit as C4array[C4ArrayIndex], and update C4ArrayIndex.
otherwise, if the cog decreased, the CompCogCode shall be shift a bit toward left from low to high; shift out the highest bit and set the C4array[C4ArrayIndex] as the new lowest one, and update C4ArrayIndex accordingly.
In which, C4ArrayIndex is the current cog index, ranging from 0 to 99. C4array[C4ArrayIndex] is the array of disc codes, "1" meaning conduction and "0" indicating blocked, for details see [REF4].
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0013]
[End]
[iTC_CC_ATP-SwRS-0164]
OdometerCogPositionReady，根据IdenticalLockedOdometer中锁存各中断的CogCode和ATP主任务计算的期望齿号CompCogCode是否匹配，判断里程计齿数齿号的可用OdometerCogPositionReady状态。
初始化时，设置OdometerCogPositionReady为False。
否则，如果之前OdometerCogPositionReady为False，则只有当ATP检测到车轮反转或者停转（WheelFilteredStopped）后重新转动，使得里程计朝同一个方向连续转过8个齿后，初始化齿数和齿号的匹配关系，并设置OdometerCogPositionReady为True。
否则，如果本周期某中断中的期望齿号CompCogCode与读到的齿号CogCode不相等，则设置OdometerCogPositionReady为False
其他情况，保持OdometerCogPositionReady不变。
ATP determines the odometer position ready according to the matching of the CompCogCode and CogCode locked in each interrupt.
In initialization, the OdometerCogPositonReady shall be False;
Or else:, if the OdometerCogPositonReady was False, then only after the odometer rotated reversely or WheelFilteredStopped and re-turned continuous toward the same direction after 8 cogs, ATP shall re-initialize the Counter-Code matching relation and set OdometerCogPositonReady as True;
Or else:, if CogCode is different with CompCogCode in one of interrupt of the cycle, ATP shall set OdometerCogPositonReady as False;
Otherwise, ATP keep OdometerCogPositonReady unchanging.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0013], [iTC_CC-SyAD-0960], [iTC_CC_ATP_SwHA-0053]
[End]
[iTC_CC_ATP-SwRS-0165]
TeethCounter，ATP根据IdenticalLockedOdometer中锁存的最后一个中断的CogCounter变化值，更新TeethCounter，作为主任务使用的里程计齿数值。TeethCounter的计算应考虑里程计安装方向和CogCounter的寄存器取值范围。
TeethCounter used as the odometer cog value in one deferred task, which is the difference of the CogCounter in the last interrupt of adjacent cycle. The calculation of the TeethCounter shall consider the installation direction of the odometer and the register range of the CogCounter.
```
	TeethCounter(k)
	 = TeethCounter(k-1)
	    + (IdenticalLockedOdometer[ATP_INTERRUPT_NB - 1].CogCounter(k)
	         - IdenticalLockedOdometer[ATP_INTERRUPT_NB - 1].CogCounter(k-1))
	       * ATPsetting.CCcoreOdoCogIncreasing[CoreId]
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0130], [iTC_CC-SyAD-0135], [iTC_CC_ATP_SwHA-0181]
[End]
NOTES:
TeethCounter是有符号值。如果TeethCounter大于0，则表示里程计相对于初始位置向列车END_1方向转动；反之如果小于0，则表示里程计相对于初始位置向列车END_2方向转动。
TeethCounter is a signed value. If TeethCounter greater than 0, then means the odometer rotating toward to the train END_1 direction; other hand, if it less than 0, then means the odometer rotating toward to the END_2.
[iTC_CC_ATP-SwRS-0166]
CogPositionBeforeTopLoc，CogPositionAfterTopLoc，如果本周期读到信标，则通过IdenticalLockedOdometer计算读到信标瞬间的里程计齿数信息：
使用Top-loc发生的前一个中断的CogCounter来更新CogPositionBeforeTopLoc；
使用Top-loc发生时中断的CogCounter来更新CogPositionAfterTopLoc；
其他情况，CogPositionBeforeTopLoc和CogPositionAfterTopLoc保持不变。
If a beacon with top-loc received in this cycle, ATP shall record the cog position of the interrupt when and just before the top-loc happen:
CogPositionBeforeTopLoc, the CogCounter in the interrupt just before the top-loc happen;
CogPositionAfterTopLoc, the CogCounter in the interrupt when the top-loc happen.
```
	CogPositionBeforeTopLoc(k)
	 = TeethCounter(k-1)
	   + ((IdenticalLockedOdometer[i-1].CogCounter(k)
	       - IdenticalLockedOdometer[ATP_INTERRUPT_NB-1].CogCounter(k-1))
	       * ATPsetting.CCcoreOdoCogIncreasing[CoreId]))
	CogPositionAfterTopLoc(k)
	 = TeethCounter(k-1)
	   + ((IdenticalLockedOdometer[i].CogCounter(k)
	        - IdenticalLockedOdometer[ATP_INTERRUPT_NB-1].CogCounter(k-1))
	      * ATPsetting.CCcoreOdoCogIncreasing[CoreId]))
```
其中i表示锁存收到Top-loc信号的那个中断。如果上下CPU收到Top-loc相差1个中断，则使用较早的的中断作为计算CogPositionBeforeTopLoc的依据，而较迟的那个中断作为计算CogPositionAfterTopLoc的依据。
Which, i means the interrupt received top-loc signal.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0197], [iTC_CC_ATP_SwHA-0059]
[End]
[iTC_CC_ATP-SwRS-0167]
SensorTestPerformed，当主任务通过锁存的IdenticalLockedOdometer数组发现本周期所有的中断均正在对传感器进行测试时，输出SensorTestPerformed为True。
否则，输出SensorTestPerformed为False。
If all interrupts in one cycle are sensors testing, ATP shall set SensorTestPerformed. Otherwise, set SensorTestPerformed as False.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0149]
[End]
[iTC_CC_ATP-SwRS-0464]
SensorSequenceDetected_1，SensorSequenceDetected_2，SensorSequenceDetected_3，
如果本周期SensorTestPerformed为True，即中断在进行传感器测试，需分别判断三路传感器的导通状态SensorSequenceDetected_1，SensorSequenceDetected_2和SensorSequenceDetected _3：
如果该路传感器在本周期所有中断的测试结果均为SENSOR_CONDUCT，则设置相应状态为True；
否则，设置相应传感器状态为False。
如果本周期SensorTestPerformed为False，则设置三路传感器状态均为False。
If sensors testing performed in this cycle, ATP shall determine the conduction state of each sensor:
If all test results of every interrupts for this sensor are SENSOR_CONDUCT, ATP shall set sensor sequence detected for this sensor;
Otherwise, does not set this sensor sequence detected.
If sensors testing do not perform, ATP does not set any sensor sequence detected.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0149]
[End]
[iTC_CC_ATP-SwRS-0168]
UnconsistentSensorTest，通过检查C1/2/3传感器的一致性，判断里程计故障。
如果在传感器测试中，任意一次中断中的传感器测试的结果为C1/2/3三路全为SENSOR_CONDUCT，或三路全为SENSOR_BLOCKED，或者任意一路为SENSOR_WRONG，则设置本周期UnconsistentSensorTest为True；
或者，在非传感器测试时，如果某中断的C1/2/3三路状态相同（同为导通或同为堵塞），也应设置本周期
UnconsistentSensorTest为True；否则，设置UnconsistentSensorTest为False。
ATP shall check consistency of sensors whether sensor testing performed or not. Sensors testing result shall declare inconsistent at cycle k (UnconsistentSensorTest) if the following conditions are fulfilled:
Sensors test done at cycle k, and no sequence has been detected on any of the three sensors C1, C2, C3 (SENSOR_BLOCKED)
Or at cycle k, the expected sequence is detected on all three sensors C1, C2, C3 (SENSOR_CONDUCT).
Or any of the three sensors is tested as error (SENSOR_WRONG).
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0149], [iTC_CC_ATP_SwHA-0035], [iTC_CC_ATP_SwHA-0057]
[End]
[iTC_CC_ATP-SwRS-0171]
WheelStopped，如果当前在进行传感器测试，且任一中断中未发生三路全通或全堵错误，且一个周期所有中断内三路传感器的导通状态都与上周期的结果相同时，输出WheelStopped为True。否则为False。
Wheel shall consider safely stopped WheelStopped at cycle k if the following conditions are fulfilled:
sensors test has been performed,
and at least one sensor out of three sensors C1, C2, C3 has detected expected sequence,
and at least one sensor out of three sensors C1, C2, C3 has not detected expected sequence,
and sensors test result combination on three sensors C1, C2, C3 has not changed between cycle k-1 and k.
```
	if (SensorTestPerformed(k) == True)
	    WheelStopped(k)
	     = ((UnconsistentSensorTest(k) == False)
	       and (SensorSequenceDetected_1 = SensorSequenceDetected_1(k-1))
	       and (SensorSequenceDetected_2 = SensorSequenceDetected_2(k-1))
	       and (SensorSequenceDetected_3 = SensorSequenceDetected_3(k-1)))
	else:
	    WheelStopped = False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0150]
[End]
[iTC_CC_ATP-SwRS-0172]
WheelFilteredStopped，判断本周期车轮是否处于滤过停止状态，规则如下：
如果WheelFilteredStopped上周期为False，而本周期WheelStopped由False变为True，则认为本周期为True。
在此条件下，记录停车时的齿数LastStopCogPosition为当前齿数
WheelFilteredStopped由True变为False的条件：
齿数移动超过1个齿
At cycle k, WheelFilteredStopped shall change from False to True on raising edge of WheelStopped information, That is, if:
WheelStopped information was False at cycle k-1,
and WheelStopped information was True at cycle k.
and then:
LastStopCogPosition is assigned to TeethCounter,
At cycle k, WheelFilteredStopped shall change from True to False, according following expression:
the cog moved more than one cog;
```
	def WheelFilteredStopped(k):
	    if (not WheelFilteredStopped(k-1)
	        and not WheelStopped(k-1)
	        and WheelStopped(k)):
	        LastStopCogPosition = TeethCounter(k)
	        return True
	    elif (WheelFilteredStopped(k-1)
	          and not UnconsistentSensorTest(k)
	          and abs(TeethCounter(k) - LastStopCogPosition) <= 1):
	        return True
	    else:
	        return False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0151], [iTC_CC-SyAD-0214]
[End]
[iTC_CC_ATP-SwRS-0173]
MaxCountCogsRunInCycleExceeded，里程计转过齿数不能超过周期最大值，也不能超过的相邻中断的最大值。
ATP shall detect whether the cog number counted in adjacent interrupt is greater than the default maximum cog number on cycle or on interrupt.
```
	def MaxCountCogsRunInCycleExceeded(k):
	    if (abs(IdenticalLockedOdometer[ATP_INTERRUPT_NB-1].CogCounter(k)
	            - IdenticalLockedOdometer[ATP_INTERRUPT_NB-1].CogCounter(k-1))
	         > ATPsetting.OdoMaxCogOnCycle):
	        return True
	    else:
	        for i in range(ATP_INTERRUPT_NB-1):
	            if (abs(IdenticalLockedOdometer[i].CogCounter
	                     - IdenticalLockedOdometer[i+1].CogCounter)> ATPsetting.OdoMaxCogOnIntrrupt):
	                return True
	            else:
	                continue
	        return False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0149], [iTC_CC-SyAD-1158], [iTC_CC_ATP_SwHA-0054]
[End]
NOTES:
ATPsetting.OdoMaxCogOnCycle，由CC离线工具将里程计允许最大速度，根据项目最大齿距转换的每周期允许转过的最大齿数；
ATPsetting.OdoMaxCogOnInterrupt，由CC离线工具将里程计允许最大速度，根据项目最小齿距转换的每中断允许转过的最大齿数。
[iTC_CC_ATP-SwRS-0174]
WheelKinematicsInvalidForCogCount，如果ATP检测到某个中断的齿数转过最大值时，设置齿数计算错误。
If the calculated movement exceeds the default one, ATP shall set the wheel kinematics invalid.
```
	WheelKinematicsInvalidForCogCount = MaxCountCogsRunInCycleExceeded(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0149], [iTC_CC-SyAD-0960], [iTC_CC-SyAD-1158], [iTC_CC_ATP_SwHA-0054]
[End]
