/*
SQLyog Ultimate v10.42 
MySQL - 5.5.5-10.1.9-MariaDB : Database - dbcargo
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`dbcargo` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `dbcargo`;

/*Table structure for table `borrowed_load` */

DROP TABLE IF EXISTS `borrowed_load`;

CREATE TABLE `borrowed_load` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) DEFAULT NULL,
  `summa` float(12,2) DEFAULT '0.00',
  `date` date DEFAULT NULL,
  `comment` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

/*Data for the table `borrowed_load` */

insert  into `borrowed_load`(`id`,`user_id`,`summa`,`date`,`comment`) values (1,2,10.00,'2025-05-17',NULL),(2,1,12.00,'2025-05-17',NULL),(3,3,89.00,'2025-05-17',NULL),(4,1,1.00,'2025-05-17',NULL),(5,2,1.00,'2025-05-17',NULL),(6,1,5.00,'2025-05-17',NULL),(7,1,6.00,'2025-05-17',NULL),(8,1,12.00,'2025-05-18',NULL);

/*Table structure for table `city` */

DROP TABLE IF EXISTS `city`;

CREATE TABLE `city` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `country_id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

/*Data for the table `city` */

insert  into `city`(`id`,`country_id`,`name`) values (1,2,'Иву'),(2,2,'Урумчи'),(3,1,'Душанбе'),(4,1,'Хуҷанд'),(5,1,'Истаравшан');

/*Table structure for table `countries` */

DROP TABLE IF EXISTS `countries`;

CREATE TABLE `countries` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `countries` */

insert  into `countries`(`id`,`name`) values (1,'Тоҷикистон'),(2,'Хитой');

/*Table structure for table `main_window` */

DROP TABLE IF EXISTS `main_window`;

CREATE TABLE `main_window` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date_input` date DEFAULT NULL,
  `date_finish` date DEFAULT NULL,
  `count` int(11) DEFAULT NULL,
  `cube` float(12,2) DEFAULT '0.00',
  `kg` float(12,2) DEFAULT '0.00',
  `city_id` int(11) DEFAULT NULL,
  `status_module` varchar(255) DEFAULT 'China',
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*Data for the table `main_window` */

/*Table structure for table `products` */

DROP TABLE IF EXISTS `products`;

CREATE TABLE `products` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `main_window_id` int(11) DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `product_name` varchar(255) DEFAULT NULL,
  `count` int(11) DEFAULT NULL,
  `cube` float(12,2) DEFAULT '0.00',
  `kg` float(12,2) DEFAULT '0.00',
  `product_condition` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*Data for the table `products` */

/*Table structure for table `rasxod1` */

DROP TABLE IF EXISTS `rasxod1`;

CREATE TABLE `rasxod1` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sana` date NOT NULL,
  `info` varchar(255) NOT NULL,
  `comment` varchar(255) DEFAULT NULL,
  `summa` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `rasxod1` */

insert  into `rasxod1`(`id`,`sana`,`info`,`comment`,`summa`) values (1,'2025-05-18','Taxi',NULL,12.00),(2,'2025-05-17','Avto',NULL,50.00);

/*Table structure for table `repaid_load` */

DROP TABLE IF EXISTS `repaid_load`;

CREATE TABLE `repaid_load` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) DEFAULT NULL,
  `summa` float(12,2) DEFAULT '0.00',
  `date` date DEFAULT NULL,
  `comment` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

/*Data for the table `repaid_load` */

insert  into `repaid_load`(`id`,`user_id`,`summa`,`date`,`comment`) values (1,2,10.00,'2025-05-17',NULL),(2,3,52.00,'2025-05-17',NULL),(3,1,12.00,'2025-05-17',NULL),(4,1,12.00,'2025-05-17',NULL),(5,2,12.00,'2025-05-17',NULL),(6,1,12.00,'2025-05-18',NULL),(7,2,13.00,'2025-05-18',NULL);

/*Table structure for table `status` */

DROP TABLE IF EXISTS `status`;

CREATE TABLE `status` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `module` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `color` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

/*Data for the table `status` */

insert  into `status`(`id`,`module`,`description`,`color`) values (1,'China','Бор аз Хитой барод','Crimson'),(2,'Kazakhstan','Бор ба Казокистод даромада мошинхо алиш шудан баьд аз Казокистон брод ','MediumSlateBlue'),(3,'Uzbekistan','Бор ба Узбекисто даромада мошинхо алиш шудан баьд аз Узбекисто брод','Yellow'),(4,'Tajikistan','Бор ба Точикистон дарод','Fuchsia'),(5,'Istaravshan warehouse','Бор ба склади Истаравшан биёд','MediumSpringGreen');

/*Table structure for table `transports` */

DROP TABLE IF EXISTS `transports`;

CREATE TABLE `transports` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `main_window_id` int(11) DEFAULT NULL,
  `date_start` date DEFAULT NULL,
  `transport_number` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*Data for the table `transports` */

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `city_id` int(11) DEFAULT NULL,
  `code` varchar(191) DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `phone_number` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_code` (`code`),
  UNIQUE KEY `code` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

/*Data for the table `users` */

insert  into `users`(`id`,`city_id`,`code`,`name`,`phone_number`) values (1,4,'cl1','Ali','+992 92 123 45 67'),(2,4,'cl2','Vali','+992 92 987 65 43'),(3,4,'cl3','G\'ani','+992 92 852 96 31'),(4,4,'4455','Abdu','+992 91 852 78 31');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
