/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50721
Source Host           : localhost:3306
Source Database       : whicheat

Target Server Type    : MYSQL
Target Server Version : 50721
File Encoding         : 65001

Date: 2018-03-20 14:20:31
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for relation_favorite
-- ----------------------------
DROP TABLE IF EXISTS `relation_favorite`;
CREATE TABLE `relation_favorite` (
  `Phone` bigint(11) NOT NULL COMMENT '收藏者手机号',
  `RecipesId` int(11) NOT NULL COMMENT '收藏食谱编号',
  `UpdateDate` datetime NOT NULL COMMENT '最后更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for relation_share
-- ----------------------------
DROP TABLE IF EXISTS `relation_share`;
CREATE TABLE `relation_share` (
  `RecipesId` int(11) NOT NULL COMMENT '食谱编号',
  `DishId` int(11) NOT NULL COMMENT '食品编号',
  `Phone` bigint(11) NOT NULL COMMENT '分享者手机号',
  `UpdateDate` datetime NOT NULL COMMENT '最后更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for which_account
-- ----------------------------
DROP TABLE IF EXISTS `which_account`;
CREATE TABLE `which_account` (
  `Phone` bigint(11) NOT NULL COMMENT '手机号码11位',
  `Email` varchar(255) COLLATE utf8_bin NOT NULL COMMENT '邮箱地址',
  `SecurityCode` varchar(255) COLLATE utf8_bin NOT NULL COMMENT '安全码(base64加密密码)',
  `CreatDate` datetime NOT NULL COMMENT '账号创建日期',
  `BirthdayDate` datetime DEFAULT NULL COMMENT '生日日期',
  `NickName` varchar(255) COLLATE utf8_bin DEFAULT '' COMMENT '账户昵称',
  `Avator` text COLLATE utf8_bin COMMENT '头像base64字符串',
  `BackCorver` text COLLATE utf8_bin COMMENT '背景墙图片base64字符串',
  `Sex` char(1) COLLATE utf8_bin NOT NULL DEFAULT '0' COMMENT '性别 0:女 1:男',
  `Description` varchar(255) COLLATE utf8_bin DEFAULT '' COMMENT '账户个人描述',
  `RequiredStatus` char(1) COLLATE utf8_bin NOT NULL DEFAULT '0' COMMENT '账户认证状态 0:未认证 1:已认证',
  PRIMARY KEY (`Phone`,`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for which_dish
-- ----------------------------
DROP TABLE IF EXISTS `which_dish`;
CREATE TABLE `which_dish` (
  `DishId` int(11) NOT NULL COMMENT '食品编号',
  `Name` varchar(255) COLLATE utf8_bin NOT NULL COMMENT '食品名称',
  `UpdateDate` datetime NOT NULL COMMENT '最后更新时间',
  `Type` int(11) NOT NULL COMMENT '类型',
  `TypeName` varchar(255) COLLATE utf8_bin NOT NULL COMMENT '类型名称',
  `ShopId` int(11) NOT NULL COMMENT '所属商店编号',
  `ShopName` varchar(255) COLLATE utf8_bin NOT NULL COMMENT '所属商店名称',
  `Image` text COLLATE utf8_bin COMMENT '配图',
  `Description` varchar(255) COLLATE utf8_bin DEFAULT NULL COMMENT '描述',
  PRIMARY KEY (`DishId`,`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for which_recipes
-- ----------------------------
DROP TABLE IF EXISTS `which_recipes`;
CREATE TABLE `which_recipes` (
  `RecipesId` int(11) NOT NULL COMMENT '食谱编号',
  `Name` varchar(255) COLLATE utf8_bin NOT NULL COMMENT '食谱名称',
  `UpdateDate` datetime NOT NULL COMMENT '最后更新时间',
  `Phone` bigint(11) NOT NULL COMMENT '创建者手机号',
  `Cover` text COLLATE utf8_bin COMMENT '封面',
  `Description` varchar(255) COLLATE utf8_bin DEFAULT NULL COMMENT '食谱描述',
  `Tag` varchar(255) COLLATE utf8_bin DEFAULT NULL COMMENT '标签',
  `IsPrivate` char(1) COLLATE utf8_bin NOT NULL DEFAULT '0' COMMENT '是否私有 0不私有 1私有',
  PRIMARY KEY (`RecipesId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for which_shop
-- ----------------------------
DROP TABLE IF EXISTS `which_shop`;
CREATE TABLE `which_shop` (
  `ShopId` int(11) NOT NULL COMMENT '商店编号',
  `ShopName` varchar(255) COLLATE utf8_bin NOT NULL COMMENT '商店名称',
  `Longitude` decimal(10,6) NOT NULL COMMENT '经度',
  `Latitude` decimal(10,6) NOT NULL COMMENT '纬度',
  `UpdateDate` datetime NOT NULL COMMENT '最后更新时间',
  `Description` varchar(255) COLLATE utf8_bin DEFAULT NULL COMMENT '商店描述'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
