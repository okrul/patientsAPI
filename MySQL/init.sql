-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: localhost    Database: api_test
-- ------------------------------------------------------
-- Server version	8.0.35

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE DATABASE `api_test`;
USE `api_test`;

--
-- Table structure for table `patient`
--


/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `patient` (
  `id` varchar(38) NOT NULL,
  `gender` varchar(45) DEFAULT NULL,
  `birthDate` timestamp NOT NULL,
  `useName` varchar(45) DEFAULT NULL,
  `familyName` varchar(45) NOT NULL,
  `givenName` varchar(90) DEFAULT NULL,
  `active` tinyint DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `patient`
--

LOCK TABLES `patient` WRITE;
/*!40000 ALTER TABLE `patient` DISABLE KEYS */;
INSERT INTO `patient` VALUES ('223828d1-cbcb-11ee-a980-b4b52f77def1','female','2024-02-19 21:00:00','official','Кузнецова','Елизавета Александровна',0),('224cc13a-cbcb-11ee-a980-b4b52f77def1','female','2024-03-08 21:00:00','official','Кузнецова','Татьяна Петровна',0),('2255b5a1-cbcb-11ee-a980-b4b52f77def1','female','2024-03-09 21:00:00','official','Иванова','Мария Викторовна',1),('2260b4a2-cbcb-11ee-a980-b4b52f77def1','male','2024-02-25 21:00:00','official','Кузнецов','Владимир Иванович',1),('22672c23-cbcb-11ee-a980-b4b52f77def1','female','2024-03-04 21:00:00','official','Иванова','Мария Владимировна',1),('226c18fa-cbcb-11ee-a980-b4b52f77def1','male','2024-02-25 21:00:00','official','Иванов','Александр Иванович',1),('226e7715-cbcb-11ee-a980-b4b52f77def1','unknown','2024-02-17 21:00:00','official','Сидоров','Петр Викторович',0),('2272ba3a-cbcb-11ee-a980-b4b52f77def1','unknown','2024-03-14 21:00:00','official','Сидоров','Виктор Александрович',1),('2275d8d0-cbcb-11ee-a980-b4b52f77def1','female','2024-03-01 21:00:00','official','Кузнецова','Ольга Петровна',0),('22796a81-cbcb-11ee-a980-b4b52f77def1','unknown','2024-02-28 21:00:00','official','Петров','Иван Владимирович',1),('32fb1611-ca89-11ee-a2b5-00090faa0001','female','2008-01-13 12:00:43','non official','Сидорова','Татьяна Олеговна',1),('89c7c74b-ca7d-11ee-a2b5-00090faa0001','49c28a1e-ca75-11ee-a2b5-00090faa0001','2024-01-13 15:25:43','official','Иванов','Иван Иванович',0),('af0d43db-cbc7-11ee-a980-b4b52f77def1','male','2024-02-13 15:25:43','official','Чижиков','Артем Николаевич',0),('af2d4dee-cbc8-11ee-a980-b4b52f77def1','male','2024-01-13 15:25:43','official','Васильева','Ольга Олеговна',1);
/*!40000 ALTER TABLE `patient` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `patient_generate_guid` BEFORE INSERT ON `patient` FOR EACH ROW BEGIN
SET NEW.id = UUID();
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-02-15 11:56:16
