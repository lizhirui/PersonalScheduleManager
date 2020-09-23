/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50713
Source Host           : localhost:3306
Source Database       : schedulemanager

Target Server Type    : MYSQL
Target Server Version : 50713
File Encoding         : 65001

Date: 2020-09-23 08:52:47
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for eventinfo
-- ----------------------------
DROP TABLE IF EXISTS `eventinfo`;
CREATE TABLE `eventinfo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedTime` datetime NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `RemindFormula` longtext NOT NULL,
  `Type` int(11) NOT NULL,
  `TaskId` int(11) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Enabled` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`) USING BTREE,
  KEY `Time` (`StartTime`,`EndTime`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for memoinfo
-- ----------------------------
DROP TABLE IF EXISTS `memoinfo`;
CREATE TABLE `memoinfo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedTime` datetime NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `ParentId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for recordinfo
-- ----------------------------
DROP TABLE IF EXISTS `recordinfo`;
CREATE TABLE `recordinfo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Time` datetime NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for taskinfo
-- ----------------------------
DROP TABLE IF EXISTS `taskinfo`;
CREATE TABLE `taskinfo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedTime` datetime NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `ParentId` int(11) NOT NULL,
  `FinishedProgress` int(11) NOT NULL,
  `TotalProgress` int(11) NOT NULL,
  `ProgressUnit` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`) USING BTREE,
  KEY `ParentId` (`ParentId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
