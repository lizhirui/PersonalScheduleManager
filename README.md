# PersonalScheduleManager  
个人日程管理，支持备忘录/树形任务管理加进度控制以及操作记录/事件管理（日期采用JavaScript脚本指定匹配函数）/日程显示

#安装  
首先安装mysql数据库，端口号保持默认
创建一个数据库，名字为schedulemanager，UTF-8，然后导入schedulemanager.sql
在mysql中创建一个新用户，用户名为schedulemanager，密码为schedulemanagerlzr，分配schedulemanager数据库的select、insert、update、delete、lock tables权限
运行 个人日程管理.exe

#软件使用帮助  
删除某项按键盘的Delete快捷键
任务内切换单任务和子任务列表通过点击鼠标右键实现
事件中若想打开脚本助手，在JavaScript代码编辑框中按F9键即可