CREATE DATABASE  IF NOT EXISTS `stahnke` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `stahnke`;
-- MySQL dump 10.13  Distrib 5.6.17, for Win64 (x86_64)
--
-- Host: localhost    Database: stahnke
-- ------------------------------------------------------
-- Server version	5.6.21-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(45) NOT NULL,
  `Password` varchar(45) NOT NULL,
  `Email` varchar(45) NOT NULL,
  `FirstName` varchar(45) NOT NULL,
  `LastName` varchar(45) DEFAULT NULL,
  `Biography` longtext,
  `AccountType` varchar(45) NOT NULL DEFAULT 'STANDARD',
  `Contact` varchar(512) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `Username_UNIQUE` (`Username`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (20,'Obfuscate','JUvqD6cBVOz09lBsG3+uQA==','iodjfogijd','dsuifhsiud','idfjgoijd','oifdjgoidjf','STANDARD',NULL),(21,'Obfuscate2','IQZjkw+AffZo7TlGLw+XgQ==','test','test',NULL,'fasdasd','STANDARD',NULL);
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sellers`
--

DROP TABLE IF EXISTS `sellers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sellers` (
  `sellerid` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `businessname` varchar(128) NOT NULL,
  `businesswebsite` varchar(128) DEFAULT NULL,
  `businessbiography` varchar(1024) DEFAULT NULL,
  `businesslocation` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`sellerid`),
  UNIQUE KEY `businessname_UNIQUE` (`businessname`),
  UNIQUE KEY `username_UNIQUE` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sellers`
--

LOCK TABLES `sellers` WRITE;
/*!40000 ALTER TABLE `sellers` DISABLE KEYS */;
/*!40000 ALTER TABLE `sellers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sessions`
--

DROP TABLE IF EXISTS `sessions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sessions` (
  `Username` varchar(45) NOT NULL,
  `SessionID` varchar(45) NOT NULL,
  `Expires` datetime NOT NULL,
  PRIMARY KEY (`SessionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sessions`
--

LOCK TABLES `sessions` WRITE;
/*!40000 ALTER TABLE `sessions` DISABLE KEYS */;
INSERT INTO `sessions` VALUES ('vinnie','030b8943-3005-4cf1-bc64-6db7f1354cad','2015-03-18 01:32:29'),('vinnie','09704924-f20b-4457-b496-c3eafce3988c','2015-03-18 01:42:18'),('Vinnie','09fe6118-fc47-43ad-bbb8-bba6d0c9ccde','2015-03-18 01:46:03'),('Obfuscate','0a36d20d-594f-48ec-820a-b41bb12a7c2b','2015-03-31 21:54:25'),('Vinnie','0b4fc079-84b9-4865-a9a2-c92b9cdf0ae3','2015-03-18 02:41:25'),('Pathogenix','0b8a5533-ff7b-48d2-bef2-1a0d212ceb4b','2015-03-18 04:30:12'),('Vinnie','0c64a8b5-f3f0-4dd9-91e7-a49d13b7ca8c','2015-03-16 06:21:39'),('Pathogenix','13abd436-cd1a-4e0e-bccc-58a9515e0b4f','2015-03-18 01:45:33'),('Vinnie','156d1ee3-df42-4800-91a5-6d9a0804e5bc','2015-03-18 01:44:28'),('Pathogenix','190fd3e6-8c59-466b-8607-97f4ee4cc9b5','2015-03-18 04:17:37'),('vinnie','1aac49d9-d6cb-4fb8-97c6-b3ae91a85918','2015-03-18 01:42:40'),('vinnie','1af30c92-e28f-4665-b4a0-5ddf13036594','2015-03-18 01:31:35'),('vinnie','2144aeaf-d36d-4d74-8d10-10e192896912','2015-03-18 01:32:08'),('vinnie','23fdbf72-0ee8-4f4b-bd03-f4049d7e0748','2015-03-18 01:40:33'),('Pathogenix','243069a8-3313-42f7-926a-249bc67c5556','2015-03-19 01:04:55'),('Vinnie','29d76827-9c45-44ba-a683-3d6ba35e7d6e','2015-03-16 06:42:38'),('vinnie','2a496dbf-946c-461c-b85e-5536f909d948','2015-03-18 01:59:05'),('Obfuscate','2b688116-04c4-4ee4-8532-e39b80fb010a','2015-03-21 02:19:59'),('Vinnie','32b74f21-32bf-4595-88bd-eeca8770bbf4','2015-03-16 06:42:17'),('Vinnie','3cf65c56-e47a-46e6-bc17-a16addd0a5f3','2015-03-16 06:38:30'),('Vinnie','42496012-3d09-406b-92fc-68f3c4c9f1a1','2015-03-16 05:23:30'),('TestABC','4a9e72c3-a2bc-47b0-9e2f-f6f9b01bbbce','2015-03-30 01:14:27'),('TestAccount','4ce72b3d-59f6-42dd-84df-21dcd43ec3a9','2015-03-31 21:23:50'),('Vinnie','4cf12f03-f560-4d0b-8988-9f56ea35b2ae','2015-03-16 06:42:36'),('vinnie','4d6d3bd6-5b8d-4f4b-b58c-5e4aa83fb015','2015-03-18 01:42:40'),('Vinnie','52334dbb-ae16-4135-b9dd-6495d3071f8e','2015-03-18 02:42:19'),('Vinnie','53ee227a-ba28-4aaf-b8c8-b4cc00b42f14','2015-03-16 06:43:10'),('Vinnie','5403e90a-f386-41e9-8a76-6fb7637a62f6','2015-03-18 01:59:31'),('obfuscate','54bdda37-f154-4faf-a728-7af5cf1fa529','2015-03-21 02:49:11'),('Vinnie','575f154f-13bf-4de9-bf83-568c6f665166','2015-03-16 06:43:09'),('Pathogenix','596dcc32-2ac7-45da-8fa1-a8f42d072403','2015-03-18 05:57:25'),('Vinnie','5cec5b3c-fadb-4049-878e-22248081609d','2015-03-16 06:24:40'),('Pathogenix','5ec5a385-530a-4c9f-98f2-685638f2f69f','2015-03-18 01:45:38'),('obfuscate','6a546512-3c63-4286-aee7-5bf4dd269e10','2015-03-21 02:48:39'),('vinnie','6c51774c-2113-4bcb-a419-04db74d24e91','2015-03-18 01:27:51'),('Pathogenix','6d4a5c89-aa22-47d1-aa7b-034e3e6e70cd','2015-03-18 01:45:42'),('Vinnie','6dd4215e-840b-456c-8a6e-73aa6fcaab37','2015-03-16 06:38:31'),('vinnie','78228138-e14f-4981-9a90-096237ba9708','2015-03-18 01:42:30'),('Vinnie','78f9af5d-3d1a-4b67-b748-201eed63c911','2015-03-16 06:42:38'),('Obfuscate','799c10f3-c6a4-4fc1-b7ba-c2a8954bc801','2015-03-30 19:32:17'),('obfuscate','7a6c3287-ce58-4c1a-b8b1-8ec2149db5b4','2015-03-21 02:49:39'),('Pathogenix','7bb7f787-a00d-4468-8666-c58e67ab52fe','2015-03-18 04:07:15'),('Obfuscate','7dfd12a2-4c7e-44c6-8ccf-f7d57214c6da','2015-03-30 19:20:06'),('Vinnie','7ffdc9c8-b9d0-43bf-8e41-5f6d15b891c4','2015-03-16 05:18:56'),('vinnie','80e8e48b-b831-4ae9-aa31-81d9b66d0863','2015-03-18 01:42:20'),('obfuscate','822b68aa-6d43-47bc-b23a-513eff0f0ac5','2015-03-21 03:15:09'),('Vinnie','8955beb8-f5b4-466e-9c8f-a83c6628dc6d','2015-03-16 06:31:59'),('Vinnie','89e1d0f7-bbc5-4883-a54f-7ff79b8bf2b5','2015-03-16 05:26:51'),('Vinnie','8adebbad-f7ae-4f84-9798-a446774239b7','2015-03-18 01:45:54'),('Obfuscate','8bc5bcb8-6a7a-45cc-bd52-85bcab629f86','2015-03-30 19:21:28'),('obfuscate','8d52fea1-c141-495a-9dbe-bcae6124b814','2015-03-24 05:19:28'),('vinnie','8db31bf7-93e6-4b08-8929-cdbbf2fb26dd','2015-03-18 01:32:23'),('TestABC','8f01cf64-7151-41b1-a5c4-9a4c449aa899','2015-03-30 01:10:22'),('Pathogenix','90a900ce-8f19-426d-9ec2-daa56a7c6bde','2015-03-18 05:57:11'),('Vinnie','932627d3-36ad-47bc-8bb2-4c4741842434','2015-03-16 04:54:18'),('NewUser','976aed0d-7e65-473f-88c9-385cddd976e0','2015-03-16 19:14:41'),('vinnie','97e31733-a132-4b57-bc8b-8a979867062a','2015-03-18 01:32:00'),('Vinnie','98696d54-5736-4377-8af9-52eb94015f0c','2015-03-18 01:44:38'),('Vinnie','993ae4c1-519c-4d4e-8ca3-09d4437a02bc','2015-03-18 01:59:39'),('Vinnie','9961fc44-40ba-446e-9d57-3dac66dc88a2','2015-03-16 06:42:30'),('vinnie','9cb4da54-f8a5-41d2-b80d-57942098b398','2015-03-18 01:27:57'),('Obfuscate','9ee5caa5-10b8-4f87-a734-291931e56f5b','2015-03-21 02:22:39'),('vinnie','a5aa52fb-822c-4dc6-ae1c-d21352b593f7','2015-03-18 01:42:18'),('Vinnie','a70cd419-6ed1-46a1-9cb0-d7c7aae9ebce','2015-03-16 06:32:01'),('Vinnie','aaddff7e-0486-4cf2-b032-65dc34b63408','2015-03-18 01:44:19'),('Vinnie','aca7c683-ab9f-41c7-a640-868257e908fc','2015-03-16 05:23:24'),('Vinnie','aeb20587-d988-4704-9e36-debb940fe775','2015-03-16 06:32:00'),('vinnie','b2743368-a3fd-4954-8546-4052f8ef007e','2015-03-18 01:42:28'),('Vinnie','b32e8bbb-3de3-4060-ae32-eb9b6188e5a0','2015-03-18 02:15:45'),('Vinnie','b6c978d0-7472-48aa-8fc0-06392d580edf','2015-03-16 04:54:13'),('Pathogenix','b8212bae-a226-4427-8827-42317d9fd5fa','2015-03-18 04:30:26'),('Vinnie','b8790595-3979-4921-9da4-e83b06ca9a80','2015-03-18 01:44:36'),('obfuscate','c0d86429-c8d5-4fcb-98fb-ee9bc7f425ef','2015-03-21 02:49:25'),('vinnie','c47ee482-e705-40a6-a7c9-f51640552c01','2015-03-18 01:42:32'),('Vinnie','c68dc3e7-8c16-47d2-a8de-aa70018e6522','2015-03-18 02:39:58'),('vinnie','c8650e99-9abe-4241-b0e7-a836919cc0a1','2015-03-18 01:40:48'),('Vinnie','c8f2b18a-7ff9-40d1-a75f-e89727fd39a0','2015-03-18 01:44:16'),('TestABC','cb93697b-157f-4d5d-9553-e635d7b835aa','2015-03-25 02:40:18'),('vinnie','cff6e075-c441-451c-a012-bec49d4845de','2015-03-18 01:28:17'),('vinnie','d12ba4f0-8572-4816-807e-88b6c813d2ac','2015-03-18 01:42:32'),('vinnie','d2c7c748-0189-401c-995e-562440702a62','2015-03-18 01:42:29'),('Obfuscate','d3dfe707-4b96-4741-bdcf-bce7f349401d','2015-03-30 22:00:19'),('Vinnie','d4c39866-eddf-442d-b3f9-b799ee31ed5f','2015-03-18 01:44:43'),('Vinnie','d62109cc-b807-4a33-b6a9-9468db8078cc','2015-03-16 06:31:54'),('Obfuscate','d99881f9-8cba-4916-bee1-89de176c7a9a','2015-03-30 02:23:49'),('Obfuscate','dd710909-1dc6-42b6-bf7d-570c37f4a3b3','2015-03-21 02:15:46'),('Vinnie','dddbdec5-18de-493f-a6dc-7a2bcf3480ae','2015-03-18 01:44:41'),('Obfuscate','e2285c86-e5ba-4a91-a563-f23f71a092d6','2015-03-21 02:05:02'),('vinnie','e6df2b63-d560-4220-8830-4fc01196b55d','2015-03-18 01:42:32'),('Vinnie','ea633339-5ac3-4507-94d7-b5b539cfec87','2015-03-18 01:46:20'),('vinnie','f02bd3cc-415c-4ec0-a449-5375f8a22721','2015-03-18 01:40:56'),('Pathogenix','f1f455f0-64ea-455f-903b-1f67bc74726f','2015-03-18 05:56:41'),('vinnie','f6475a86-86de-4a01-a8d9-dd51238d2875','2015-03-18 01:59:31'),('Vinnie','f89970a2-6d97-4e89-9a59-fbb316e2e45a','2015-03-16 04:37:18');
/*!40000 ALTER TABLE `sessions` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-03-24 23:19:44
