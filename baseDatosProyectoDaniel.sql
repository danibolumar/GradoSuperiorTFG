CREATE DATABASE  IF NOT EXISTS `instalaciones_deportivas` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `instalaciones_deportivas`;
-- MySQL dump 10.13  Distrib 8.0.21, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: instalaciones_deportivas
-- ------------------------------------------------------
-- Server version	5.7.30-log

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

--
-- Table structure for table `dia`
--

DROP TABLE IF EXISTS `dia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dia` (
  `iddia` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(45) NOT NULL,
  PRIMARY KEY (`iddia`),
  UNIQUE KEY `descripcion_UNIQUE` (`descripcion`),
  UNIQUE KEY `iddia_UNIQUE` (`iddia`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dia`
--

LOCK TABLES `dia` WRITE;
/*!40000 ALTER TABLE `dia` DISABLE KEYS */;
INSERT INTO `dia` VALUES (7,'Domingo'),(4,'Jueves'),(1,'Lunes'),(2,'Martes'),(3,'Miercoles'),(6,'Sabado'),(5,'Viernes');
/*!40000 ALTER TABLE `dia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `horario`
--

DROP TABLE IF EXISTS `horario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `horario` (
  `idhorario` int(11) NOT NULL AUTO_INCREMENT,
  `hora_inicio` time NOT NULL,
  `hora_fin` time NOT NULL,
  PRIMARY KEY (`idhorario`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `horario`
--

LOCK TABLES `horario` WRITE;
/*!40000 ALTER TABLE `horario` DISABLE KEYS */;
INSERT INTO `horario` VALUES (1,'08:00:00','09:00:00'),(2,'09:00:00','10:00:00'),(3,'10:00:00','11:00:00'),(4,'11:00:00','12:00:00'),(5,'12:00:00','13:00:00'),(6,'13:00:00','14:00:00'),(7,'15:00:00','16:00:00'),(8,'16:00:00','17:00:00'),(9,'17:00:00','18:00:00'),(10,'18:00:00','19:00:00'),(11,'19:00:00','20:00:00'),(12,'20:00:00','21:00:00'),(13,'14:00:00','15:00:00');
/*!40000 ALTER TABLE `horario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `instalacion`
--

DROP TABLE IF EXISTS `instalacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `instalacion` (
  `idinstalacion` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  `insalacion_cerrada` tinyint(4) DEFAULT NULL,
  `idtipo` int(11) NOT NULL,
  PRIMARY KEY (`idinstalacion`),
  UNIQUE KEY `nombre_pista_UNIQUE` (`nombre`),
  UNIQUE KEY `idpista_UNIQUE` (`idinstalacion`),
  KEY `fk_PISTA_instalaciones1_idx` (`idtipo`),
  CONSTRAINT `fk_PISTA_instalaciones1` FOREIGN KEY (`idtipo`) REFERENCES `tipo_instalacion` (`idtipo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `instalacion`
--

LOCK TABLES `instalacion` WRITE;
/*!40000 ALTER TABLE `instalacion` DISABLE KEYS */;
INSERT INTO `instalacion` VALUES (1,'pista_futbol1',NULL,1),(2,'pista_futbol2',NULL,1),(3,'pista_futbol3',1,1),(4,'pista_tenis12',1,2),(5,'pista_tenis2',NULL,2),(6,'pista_tenis3',NULL,2),(7,'pista_baloncesto1',1,3),(8,'pista_baloncesto2',1,3),(9,'pista_baloncesto3',NULL,3),(10,'prueba',NULL,3),(11,'prueba11',1,2),(12,'prueba1_prueba1',1,9),(13,'aroo',0,9),(14,'pruebaFin',NULL,3),(15,'prueba15',1,1),(16,'pista',0,30),(17,'prueba1',0,17),(18,'probando',0,2),(19,'instalacion prueba',0,2);
/*!40000 ALTER TABLE `instalacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `instalaciones_tiene_horario`
--

DROP TABLE IF EXISTS `instalaciones_tiene_horario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `instalaciones_tiene_horario` (
  `instalaciones_idinstalaciones` int(11) NOT NULL,
  `dia_iddia` int(11) NOT NULL,
  `horario_idhorario` int(11) NOT NULL,
  PRIMARY KEY (`instalaciones_idinstalaciones`,`dia_iddia`,`horario_idhorario`),
  KEY `fk_instalaciones_has_horario_horario1_idx` (`horario_idhorario`),
  KEY `fk_instalaciones_has_horario_instalaciones1_idx` (`instalaciones_idinstalaciones`),
  KEY `fk_instalaciones_tiene_horario_dia1_idx` (`dia_iddia`),
  CONSTRAINT `fk_instalaciones_has_horario_horario1` FOREIGN KEY (`horario_idhorario`) REFERENCES `horario` (`idhorario`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_instalaciones_has_horario_instalaciones1` FOREIGN KEY (`instalaciones_idinstalaciones`) REFERENCES `tipo_instalacion` (`idtipo`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_instalaciones_tiene_horario_dia1` FOREIGN KEY (`dia_iddia`) REFERENCES `dia` (`iddia`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `instalaciones_tiene_horario`
--

LOCK TABLES `instalaciones_tiene_horario` WRITE;
/*!40000 ALTER TABLE `instalaciones_tiene_horario` DISABLE KEYS */;
INSERT INTO `instalaciones_tiene_horario` VALUES (1,1,1),(1,2,1),(1,3,1),(1,4,1),(1,5,1),(1,6,1),(1,7,1),(2,1,1),(2,2,1),(2,3,1),(2,4,1),(2,5,1),(2,6,1),(2,7,1),(3,1,1),(3,2,1),(3,3,1),(3,4,1),(3,5,1),(3,6,1),(3,7,1),(9,1,1),(9,3,1),(11,4,1),(16,4,1),(16,7,1),(22,1,1),(28,1,1),(29,2,1),(30,7,1),(31,1,1),(32,4,1),(1,1,2),(1,2,2),(1,3,2),(1,4,2),(1,5,2),(1,6,2),(1,7,2),(2,1,2),(2,2,2),(2,3,2),(2,4,2),(2,5,2),(2,6,2),(2,7,2),(3,1,2),(3,2,2),(3,3,2),(3,4,2),(3,5,2),(3,6,2),(3,7,2),(9,1,2),(9,3,2),(16,4,2),(16,7,2),(26,1,2),(30,7,2),(1,1,3),(1,2,3),(1,3,3),(1,4,3),(1,5,3),(1,6,3),(1,7,3),(2,1,3),(2,2,3),(2,3,3),(2,4,3),(2,5,3),(2,6,3),(2,7,3),(3,1,3),(3,2,3),(3,3,3),(3,4,3),(3,5,3),(3,6,3),(3,7,3),(9,1,3),(16,4,3),(16,7,3),(30,7,3),(1,1,4),(1,2,4),(1,3,4),(1,4,4),(1,5,4),(1,6,4),(1,7,4),(2,1,4),(2,2,4),(2,3,4),(2,4,4),(2,5,4),(2,6,4),(2,7,4),(3,1,4),(3,2,4),(3,3,4),(3,4,4),(3,5,4),(3,6,4),(3,7,4),(9,1,4),(16,4,4),(16,7,4),(30,7,4),(1,1,5),(1,2,5),(1,3,5),(1,4,5),(1,5,5),(1,6,5),(1,7,5),(2,1,5),(2,2,5),(2,3,5),(2,4,5),(2,5,5),(2,6,5),(2,7,5),(3,1,5),(3,2,5),(3,3,5),(3,4,5),(3,5,5),(3,6,5),(3,7,5),(9,1,5),(16,4,5),(16,7,5),(30,7,5),(1,1,6),(1,2,6),(1,3,6),(1,4,6),(1,5,6),(1,6,6),(1,7,6),(2,1,6),(2,2,6),(2,3,6),(2,4,6),(2,5,6),(2,6,6),(2,7,6),(3,1,6),(3,2,6),(3,3,6),(3,4,6),(3,5,6),(3,6,6),(3,7,6),(9,1,6),(16,4,6),(16,7,6),(30,7,6),(1,1,7),(1,2,7),(1,3,7),(1,4,7),(1,5,7),(2,1,7),(2,2,7),(2,3,7),(2,4,7),(2,5,7),(3,1,7),(3,2,7),(3,3,7),(3,4,7),(3,5,7),(9,1,7),(16,4,7),(16,7,7),(30,7,7),(1,1,8),(1,2,8),(1,3,8),(1,4,8),(1,5,8),(2,1,8),(2,2,8),(2,3,8),(2,4,8),(2,5,8),(3,1,8),(3,2,8),(3,3,8),(3,4,8),(3,5,8),(9,1,8),(16,4,8),(16,7,8),(30,7,8),(1,1,9),(1,2,9),(1,3,9),(1,4,9),(1,5,9),(2,1,9),(2,2,9),(2,3,9),(2,4,9),(2,5,9),(3,1,9),(3,2,9),(3,3,9),(3,4,9),(3,5,9),(9,1,9),(16,4,9),(16,7,9),(30,7,9),(1,1,10),(1,2,10),(1,3,10),(1,4,10),(1,5,10),(2,1,10),(2,2,10),(2,3,10),(2,4,10),(2,5,10),(3,1,10),(3,2,10),(3,3,10),(3,4,10),(3,5,10),(9,1,10),(16,4,10),(16,7,10),(30,7,10),(1,1,11),(1,2,11),(1,3,11),(1,4,11),(1,5,11),(2,1,11),(2,2,11),(2,3,11),(2,4,11),(2,5,11),(3,1,11),(3,2,11),(3,3,11),(3,4,11),(3,5,11),(9,1,11),(16,4,11),(16,7,11),(30,7,11),(1,1,12),(1,2,12),(1,3,12),(1,4,12),(1,5,12),(2,1,12),(2,2,12),(2,3,12),(2,4,12),(2,5,12),(3,1,12),(3,2,12),(3,3,12),(3,4,12),(3,5,12),(9,1,12),(16,4,12),(16,7,12),(30,7,12),(1,1,13),(1,2,13),(1,3,13),(1,4,13),(1,5,13),(2,1,13),(2,2,13),(2,3,13),(2,4,13),(2,5,13),(3,1,13),(3,2,13),(3,3,13),(3,4,13),(3,5,13),(16,4,13),(16,7,13),(30,7,13);
/*!40000 ALTER TABLE `instalaciones_tiene_horario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permisos`
--

DROP TABLE IF EXISTS `permisos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permisos` (
  `idpermisos` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(45) NOT NULL,
  PRIMARY KEY (`idpermisos`),
  UNIQUE KEY `idpermisos_UNIQUE` (`idpermisos`),
  UNIQUE KEY `descripcion_UNIQUE` (`descripcion`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permisos`
--

LOCK TABLES `permisos` WRITE;
/*!40000 ALTER TABLE `permisos` DISABLE KEYS */;
INSERT INTO `permisos` VALUES (10,'Alta/Baja/Modificacion de roles y permisos'),(5,'Alta/Modificacion socios'),(6,'Baja de socios'),(4,'Cambiar contraseña'),(2,'Cancelar Reserva'),(22,'Crear/Modificar/Borrar instalacion'),(21,'Crear/Modificar/Borrar tipo instalación'),(9,'Modificar la contraseña de otros'),(11,'Operaciones de Importacion y Exportacion'),(7,'Realizar informes'),(1,'Realizar reserva'),(3,'Ver reservas'),(8,'Visualizar gráficos');
/*!40000 ALTER TABLE `permisos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reserva`
--

DROP TABLE IF EXISTS `reserva`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reserva` (
  `idreserva` int(11) NOT NULL AUTO_INCREMENT,
  `fecha_reserva` date NOT NULL,
  `hora_inicio` time NOT NULL,
  `hora_fin` time NOT NULL,
  `precio` double NOT NULL,
  `no_presentado` tinyint(4) NOT NULL,
  `anulado` tinyint(4) NOT NULL,
  `pagado` tinyint(4) NOT NULL,
  `instalacion_idinstalacion` int(11) NOT NULL,
  `usuario_idusuario` int(11) NOT NULL,
  PRIMARY KEY (`idreserva`),
  UNIQUE KEY `idreserva_UNIQUE` (`idreserva`),
  KEY `fk_reserva_PISTA1_idx` (`instalacion_idinstalacion`),
  KEY `fk_reserva_USUARIO1_idx` (`usuario_idusuario`),
  CONSTRAINT `fk_reserva_PISTA1` FOREIGN KEY (`instalacion_idinstalacion`) REFERENCES `instalacion` (`idinstalacion`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_reserva_USUARIO1` FOREIGN KEY (`usuario_idusuario`) REFERENCES `usuario` (`idusuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reserva`
--

LOCK TABLES `reserva` WRITE;
/*!40000 ALTER TABLE `reserva` DISABLE KEYS */;
INSERT INTO `reserva` VALUES (1,'2020-07-25','09:00:00','10:00:00',2,1,0,1,1,3),(2,'2020-06-30','11:00:00','13:00:00',3,0,0,1,1,11),(3,'2021-06-04','17:00:00','19:00:00',4,0,0,1,2,1),(4,'2021-06-04','11:00:00','13:00:00',2,0,1,0,2,3),(5,'2021-06-04','09:00:00','10:00:00',2,1,0,1,3,3),(6,'2021-06-04','09:00:00','10:00:00',2,0,0,1,2,1),(7,'2021-06-04','08:00:00','09:00:00',2,0,0,1,1,1),(8,'2021-06-04','08:00:00','09:00:00',2,1,0,1,2,18),(9,'2021-06-04','08:00:00','09:00:00',2,0,0,1,3,1),(10,'2021-06-04','10:00:00','14:00:00',8,0,0,1,3,1),(11,'2021-06-04','09:00:00','10:00:00',2,0,0,1,1,19),(12,'2021-06-04','09:00:00','10:00:00',2,0,0,1,2,3),(13,'2021-06-04','10:00:00','14:00:00',8,0,0,1,1,18),(14,'2021-06-04','10:00:00','11:00:00',2,0,0,1,2,18),(15,'2021-06-04','12:00:00','14:00:00',7,0,0,1,4,13),(16,'2021-06-04','09:00:00','11:00:00',7,0,0,1,4,13),(17,'2021-06-04','08:00:00','10:00:00',7,0,0,1,5,13),(18,'2021-06-04','10:00:00','14:00:00',8,0,0,1,3,1),(19,'2021-06-04','08:00:00','09:00:00',2,0,0,1,2,1),(20,'2021-06-04','08:00:00','10:00:00',4,0,0,1,1,18),(21,'2021-06-04','08:00:00','12:00:00',8,0,0,1,1,18),(22,'2021-06-04','08:00:00','09:00:00',2,0,0,1,2,1),(23,'2021-06-04','09:00:00','10:00:00',4,0,0,1,7,1),(24,'2021-06-04','09:00:00','11:00:00',9.12,1,0,1,12,19),(25,'2021-06-02','20:00:00','21:00:00',2,0,0,1,1,18),(26,'2021-06-02','08:00:00','21:00:00',12,0,0,1,15,1),(27,'2021-06-02','08:00:00','11:00:00',6,0,0,1,1,1),(28,'2021-06-02','09:00:00','15:00:00',12,0,0,1,2,1),(29,'2021-06-02','09:00:00','15:00:00',12,0,0,1,3,1),(30,'2021-06-02','08:00:00','09:00:00',2,0,0,1,2,1),(31,'2021-06-02','17:00:00','18:00:00',2,0,0,1,1,1),(32,'2021-06-04','18:00:00','20:00:00',4,0,1,0,1,21),(33,'2021-06-04','20:00:00','21:00:00',2,0,0,1,1,21),(34,'2021-06-04','09:00:00','12:00:00',6,0,0,1,1,21),(35,'2021-06-04','09:00:00','12:00:00',6,0,0,1,2,1),(36,'2021-05-31','08:00:00','10:00:00',7,0,0,1,5,1),(37,'2021-06-07','18:00:00','20:00:00',4,0,0,1,1,1),(38,'2021-06-07','19:00:00','21:00:00',7,0,0,1,5,1),(39,'2021-06-07','17:00:00','18:00:00',4,0,0,1,9,1),(40,'2021-06-07','20:00:00','21:00:00',2,0,0,1,1,1),(41,'2021-06-07','20:00:00','21:00:00',2,0,0,1,2,1),(42,'2021-06-07','20:00:00','21:00:00',3.5,0,0,1,6,1),(43,'2021-06-07','20:00:00','21:00:00',3.5,0,0,1,18,1);
/*!40000 ALTER TABLE `reserva` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rol`
--

DROP TABLE IF EXISTS `rol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rol` (
  `idrol` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`idrol`),
  UNIQUE KEY `idrol_UNIQUE` (`idrol`),
  UNIQUE KEY `nombre_UNIQUE` (`nombre`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rol`
--

LOCK TABLES `rol` WRITE;
/*!40000 ALTER TABLE `rol` DISABLE KEYS */;
INSERT INTO `rol` VALUES (1,'Administrador'),(2,'Encargado'),(5,'fdfssdsd'),(11,'prueba'),(6,'prueba1'),(8,'prueba2'),(9,'prueba3'),(10,'pruebaMVVM'),(7,'pruebas'),(3,'Socio');
/*!40000 ALTER TABLE `rol` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rol_permisos`
--

DROP TABLE IF EXISTS `rol_permisos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rol_permisos` (
  `rol_idrol` int(11) NOT NULL,
  `permisos_idpermisos` int(11) NOT NULL,
  PRIMARY KEY (`rol_idrol`,`permisos_idpermisos`),
  KEY `fk_permisos_has_rol_rol1_idx` (`rol_idrol`),
  KEY `fk_permisos_has_rol_permisos1_idx` (`permisos_idpermisos`),
  CONSTRAINT `fk_permisos_has_rol_permisos1` FOREIGN KEY (`permisos_idpermisos`) REFERENCES `permisos` (`idpermisos`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_rol_has_rol_permisos1` FOREIGN KEY (`rol_idrol`) REFERENCES `rol` (`idrol`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rol_permisos`
--

LOCK TABLES `rol_permisos` WRITE;
/*!40000 ALTER TABLE `rol_permisos` DISABLE KEYS */;
INSERT INTO `rol_permisos` VALUES (1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),(1,8),(1,9),(1,10),(1,11),(1,21),(1,22),(2,1),(2,2),(2,3),(2,4),(2,5),(2,22),(3,1),(3,2),(3,3),(3,4),(3,5),(5,1),(6,1),(6,7),(7,4),(8,4),(9,22),(10,8),(11,6);
/*!40000 ALTER TABLE `rol_permisos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `socios`
--

DROP TABLE IF EXISTS `socios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `socios` (
  `idsocios` int(11) NOT NULL AUTO_INCREMENT,
  `observaciones` varchar(200) DEFAULT NULL,
  `penalizado` tinyint(4) DEFAULT NULL,
  `fecha_fin_penalizacion` date DEFAULT NULL,
  `usuario_idusuario` int(11) NOT NULL,
  PRIMARY KEY (`idsocios`),
  UNIQUE KEY `idsocios_UNIQUE` (`idsocios`),
  KEY `fk_socios_usuario1_idx` (`usuario_idusuario`),
  CONSTRAINT `fk_socios_usuario1` FOREIGN KEY (`usuario_idusuario`) REFERENCES `usuario` (`idusuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `socios`
--

LOCK TABLES `socios` WRITE;
/*!40000 ALTER TABLE `socios` DISABLE KEYS */;
INSERT INTO `socios` VALUES (13,'esta es para el socio1',1,'2021-06-19',19),(14,NULL,NULL,NULL,21),(15,NULL,1,'2021-06-19',18);
/*!40000 ALTER TABLE `socios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_instalacion`
--

DROP TABLE IF EXISTS `tipo_instalacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_instalacion` (
  `idtipo` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(80) NOT NULL,
  `tiempo_max` int(11) NOT NULL,
  `precio_hora` double NOT NULL,
  PRIMARY KEY (`idtipo`),
  UNIQUE KEY `descripcion_UNIQUE` (`descripcion`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_instalacion`
--

LOCK TABLES `tipo_instalacion` WRITE;
/*!40000 ALTER TABLE `tipo_instalacion` DISABLE KEYS */;
INSERT INTO `tipo_instalacion` VALUES (1,'Futbol11',10,2),(2,'Tenis',2,3.5),(3,'Baloncesto',2,4),(9,'prueba1sus',5,4.56),(11,'prueba2',5,4.56),(12,'prueba23',5,4.56),(13,'prueba3',3,4.56),(14,'prueba4',3,5.45),(16,'fdgf',3,4.65),(17,'gfdgf',3,5.45),(18,'juan',3,4.65),(19,'pruebasSiNo?',5,4.55),(20,'pruebas',4,5.45),(21,'pruebasgg',3,4.54),(22,'prueba?',4,4.51),(26,'vamono',4,5.45),(28,'funciona',4,4.54),(29,'tupudfsdaf',5,5.45),(30,'prueba tipo',13,1.11),(31,'sdfsdfas',3,5.46),(32,'sdfdsgdfggf',2,4.45);
/*!40000 ALTER TABLE `tipo_instalacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `idusuario` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  `apellido1` varchar(45) NOT NULL,
  `apellido2` varchar(45) DEFAULT NULL,
  `contraseña` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `cuenta_bancaria` varchar(45) NOT NULL,
  `fecha_alta` date NOT NULL,
  `fecha_baja` date DEFAULT NULL,
  `rol_idrol` int(11) NOT NULL,
  PRIMARY KEY (`idusuario`),
  UNIQUE KEY `idusuario_UNIQUE` (`idusuario`),
  UNIQUE KEY `dni_UNIQUE` (`dni`),
  KEY `fk_USUARIO_rol1_idx` (`rol_idrol`),
  CONSTRAINT `fk_USUARIO_rol1` FOREIGN KEY (`rol_idrol`) REFERENCES `rol` (`idrol`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Daniel','Bolumar','ss','admin','+34 - 123 211 221','49350139M','LT12 1000 0111 0100 1000','2021-05-04',NULL,1),(2,'sdfjuan','sdsd',NULL,'encargado','+34 - 446 565 656','12345678C','fdgdfgd','2021-05-04',NULL,6),(3,'Manue','sfsd','dfs','socio','+34 - 564 644 551','87654321V','dfsfsddsf4','2021-05-04','2021-05-23',5),(11,'ghfghgsdfrsfsd','sdfdssf',NULL,'hola','+34 - 565 664 565','49350137A','LT12 1000 0111 0100 1000','2021-05-07','2021-05-08',1),(13,'encargfdg','ddsdsf',NULL,'H','+34 - 354 656 543','19369474R','LT12 1000 0111 0100 1000','2021-05-07','2021-05-08',2),(16,'asfdsf','sdfsdfds',NULL,'h','+34 - 454 545 645','64899882T','LT12 1000 0111 0100 1000','2021-05-08','2021-05-08',3),(17,'socioloco','sdfsdfs',NULL,'hola','+34 - 541 561 521','09779931D','IE29 AIBK 9311 5212 3456 78','2021-05-08','2021-05-29',2),(18,'socioGG','dsdf',NULL,'PEHBFpIF4r','+34 - 454 898 455','43305833F','LT12 1000 0111 0100 1000','2021-05-09','2021-05-29',3),(19,'socio1','socio1',NULL,'buenas','+34 - 456 415 123','85941579D','IE29 AIBK 9311 5212 3456 78','2021-05-27',NULL,3),(20,'borrar','dsf',NULL,'dfs','456','fs6f1','516fsd','2021-05-29','2021-05-29',1),(21,'socio','dsfasd',NULL,'hola','+34 - 448 778 494','18972849B','LT12 1000 0111 0100 1000','2021-05-30',NULL,3),(23,'socioP','s',NULL,'hola','+34 - 456 458 864','96140458K','LT12 1000 0111 0100 1000','2021-06-07','2021-06-07',3);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-07 19:47:13
