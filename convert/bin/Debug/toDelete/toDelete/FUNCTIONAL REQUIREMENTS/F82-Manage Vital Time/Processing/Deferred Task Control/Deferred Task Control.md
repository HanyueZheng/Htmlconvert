
[iTC_CC_ATP-SwRS-0535]
SafeTimerFailed，判断硬件定时中断是否正确。在主任务中检查每个相邻中断中锁存的VLE安全时钟脉冲数是否在误差范围[MIN_TIMER_IMPULSE_NB, MAX_TIMER_IMPULSE_NB]内。
一旦判断SafeTimerFailed为True，则始终保持为True（只有重启ATP才能缓解）；
否则，若测得脉冲数在上述范围内，则设置SafeTimerFailed为False;
否则，设置SafeTimerFailed为True。
The SafeTimerFailed defines whether the fix-time interrupt for hardware is correct or not. ATP shall check whether the safe clock impulse number with the adjacent interrupt is within the error range [MIN_TIMER_IMPULSE_NB, MAX_TIMER_IMPULSE_NB].
Once the SafeTimerFailed was True, ATP shall keep it as True unless the system is rebooted.
Or else:, if the impulse number is within the above-mentioned range, ATP shall set SafeTimerFailed as False
Otherwise, it will set SafeTimerFailed as True. 
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0012], [iTC_CC_ATP_SwHA-0172]
[End]
[iTC_CC_ATP-SwRS-0047]
VitalTime，ATP主任务维护的当前周期序号。上电后从0开始，每周期递增加1。当主任务在执行完成本周期的所有工作后，监控中断任务是否执行完成，即ImmediateCounter和LockedImmediateCounter的差值是否大于等于（ATP_INTERRUPT_NB -1）：
若是，则表明主周期执行完成：
将VitalTime送给另一个CPU模块；
将Trace(k)和Dt(k)作为校核字送给VIOM进行校验。
在本周期最后，设置
```
	VitalTime = VitalTime(k-1) + 1
```
否则，继续等待。
The VitalTime stand for the current cycle of ATP deferred task. After power up, it starts from zero and increase one each cycle. When all the work is executed in the main task, ATP detects whether the interrupt task is over, i.e. the difference between ImmediateCounter and LockedImmediateCounter is equal to or larger than (ATP_INTERRUPT_NB -1). 
If it is so, it shows that the main task in this cycle finishes. Then ATP shall:
send the VitalTime to the other CPU,
and send Trace and Dt to VIOM to check,
and at the end of this cycle, set
```
	 VitalTime = VitalTime(k-1) + 1
```
Otherwise, keep waiting.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0115], [iTC_CC_ATP_SwHA-0004]
[End]
[iTC_CC_ATP-SwRS-0048]
当满足下列条件时，执行新的主周期：
CycleSynchronized = True,
and TOC_VitalTime == VitalTime(k)，表示另一个CPU执行已执行完成上个周期的任务。
如果不满足上述条件，则不允许执行新周期，CPU1的ATP在VLE板的LED上显示ERR_SYNCH信息。
ATP executes the new cycle DeferredTask when below conditions fulfilled:
CycleSynchronized is True,
and the TOC_VitalTime get from the other CPU is equal to VitalTime, representing that the other CPU finished to execute the task in the previous cycle. 
If above condition does not fulfill, ATP shall prohibit to execute, and CPU1 shows the message ERR_SYNCH in the LED of VLE board. 
\#Category= Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0831], [iTC_CC_ATP_SwHA-0003], [iTC_CC-SyAD-
1423][End]
 Figure 523 Synchronize cycle sequence between two CPUs
NOTES: 
如Figure 523为双CPU模块周期同步的示意图。如果在Cycle N时，CPU1的时钟快于CPU2，则CPU1的中断计数器提前数到ATP_INTERRUPT_NB，则向CPU2发送CPU1_VitalTime(N)信息，但此时CPU2的中断计数器尚未到新周期，则此时两个CPU模块均不能进入Cycle(N+1)。只有等CPU2接到下个周期的开始标志之后，才能进入Cycle(N+1)，同时，发送CPU2_VitalTime(N)信号给CPU1，通知CPU1进入新周期。而此时，CPU1则忽略一个中断时长。即相当于CPU1“等待”了CPU2一段时间，起到了同步作用。
As shown in Figure 523, this is the demonstration of the process for cycle synchronization between two CPU modules. In cycle N, if the clock of CPU1 was faster than CPU2, then the interrupt counter of CPU1 counts to ATP_INTERRUPT_NB and sends the CPU1_VitalTime(N) information to CPU2. However, at this moment the interrupt counter of CPU2 still do not achieve the new cycle, so both of the CPU module cannot enter into cycle(N+1). Only when the CPU2 has get all interrupt finished signal, then sends the VitalTime(N) to CPU1, and the both CPUs are enter the cycle(N+1) together. During this process, CPU1 has ignored one interrupt period to wait the CPU2.
这种趋向于“等待”的同步方式，会避免时钟“变快”的危险。就是说，可能将(ATP_CYCLE_TIME+ATP_INTERRUPT_TIME)时间内测得的位移，除以ATP_CYCLE_TIME时间，则得到的速度是比实际要大的，即过估了列车速度，从而保证安全。
This kind of "waiting" synchronous way will avoid the danger that the clock is getting faster and faster. That is to say, we may use the measured movement in the period (ATP_CYCLE_TIME + ATP_INTERRUPT_TIME) divide a fixed ATP_CYCLE_TIME, then we can get speed which is higher than the actual value, meaning that we over-evaluate the train speed so as to ensure the safety. 
CPU1和CPU2的周期误差不能超过1次中断时长ATP_INTERRUPT_TIME，如果超过了该时间，则设置CycleSynchronized为False，而ATP将不会进入执行新的主周期。从而导致VIOM切断对车辆的输出。
The cycle error between CPU1 and CPU2 cannot exceed one ATP_INTERRUPT_TIME. If it exceeded this period, the CycleSynchronized shall be set as False, and the ATP will not execute continuously so that the VIOM will cut off the output to the train. 
[iTC_CC_ATP-SwRS-0589]
为确保ATP与CCNV的周期同步，ATP应当每CCNV_CYCLE_TIME触发一次DVCOM-2板的中断。
\#Category= Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1134]
[End]
