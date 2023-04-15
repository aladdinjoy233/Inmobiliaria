-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 15, 2023 at 09:09 PM
-- Server version: 10.4.24-MariaDB
-- PHP Version: 7.4.29

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Table structure for table `contratos`
--

CREATE TABLE `contratos` (
  `id_contrato` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `monto_mensual` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `contratos`
--

INSERT INTO `contratos` (`id_contrato`, `id_inmueble`, `id_inquilino`, `fecha_inicio`, `fecha_fin`, `monto_mensual`) VALUES
(2, 1, 4, '2023-04-03', '2023-04-05', '5000');

-- --------------------------------------------------------

--
-- Table structure for table `enum_tipos`
--

CREATE TABLE `enum_tipos` (
  `id_tipo` int(11) NOT NULL,
  `nombre_tipo` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `enum_tipos`
--

INSERT INTO `enum_tipos` (`id_tipo`, `nombre_tipo`) VALUES
(1, 'Casa'),
(2, 'Departamento'),
(3, 'Oficina'),
(4, 'Local'),
(5, 'Terreno'),
(6, 'Galpon'),
(7, 'Edificio'),
(8, 'Hotel'),
(9, 'Quinta');

-- --------------------------------------------------------

--
-- Table structure for table `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id_inmueble` int(11) NOT NULL,
  `id_propietario` int(11) NOT NULL,
  `direccion` varchar(45) NOT NULL,
  `uso` int(11) DEFAULT NULL,
  `tipo` int(11) DEFAULT NULL,
  `ambientes` int(11) NOT NULL,
  `latitud` decimal(10,0) NOT NULL,
  `longitud` decimal(10,0) NOT NULL,
  `precio` decimal(10,0) NOT NULL,
  `activo` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `inmuebles`
--

INSERT INTO `inmuebles` (`id_inmueble`, `id_propietario`, `direccion`, `uso`, `tipo`, `ambientes`, `latitud`, `longitud`, `precio`, `activo`) VALUES
(1, 1, 'Por alla', 1, 1, 5, '10', '10', '5000', 1),
(4, 6, 'testeando', 2, 8, 15, '5', '5', '500000', 0),
(6, 4, 'Concaran', 1, 1, 4, '1', '1', '500', 1),
(7, 16, 'Av. Qsyo', 1, 1, 3, '123', '321', '400', 1);

-- --------------------------------------------------------

--
-- Table structure for table `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id_inquilino` int(11) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `correo` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `inquilinos`
--

INSERT INTO `inquilinos` (`id_inquilino`, `dni`, `apellido`, `nombre`, `telefono`, `correo`) VALUES
(4, 'test', 'test', 'test', 'test', 'test'),
(6, '123', '321', '123', '321', '321'),
(8, '123456789', 'Ibarra', 'Hugo', '123456', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `pagos`
--

CREATE TABLE `pagos` (
  `id_pago` int(11) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `numero` int(11) NOT NULL,
  `fecha` date NOT NULL,
  `importe` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `pagos`
--

INSERT INTO `pagos` (`id_pago`, `id_contrato`, `numero`, `fecha`, `importe`) VALUES
(1, 2, 1, '2023-03-09', '5000'),
(3, 2, 2, '2023-04-09', '5000'),
(5, 2, 3, '2023-04-15', '5000');

-- --------------------------------------------------------

--
-- Table structure for table `propietarios`
--

CREATE TABLE `propietarios` (
  `id_propietario` int(11) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `correo` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `propietarios`
--

INSERT INTO `propietarios` (`id_propietario`, `dni`, `apellido`, `nombre`, `telefono`, `correo`) VALUES
(1, '95000000', 'Chica', 'Allan', '2664', 'ninjalover249@gmail.com'),
(4, '23000000', 'Suares', 'Nica', '2663', 'nica@nica.com'),
(6, '12345', 'test', 'test', 'test', 'test'),
(7, '1234', 'creacion', 'test', 'test', 'test@test.com'),
(8, '12', 'test', 'testing', '123', NULL),
(9, 'test1', '1', '1', '1', '1'),
(10, 'test2', '2', '2', '2', '2'),
(11, 'test3', '3', '3', '3', '3'),
(16, '21000000', 'Palermo', 'Martin', '12345', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `usuarios`
--

CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `email` varchar(50) NOT NULL,
  `password` longtext NOT NULL,
  `avatar` longtext DEFAULT NULL,
  `rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `usuarios`
--

INSERT INTO `usuarios` (`id_usuario`, `nombre`, `apellido`, `email`, `password`, `avatar`, `rol`) VALUES
(1, 'Allan', 'Chica', 'ninjalover249@gmail.com', 'a8m9KfoAdA5bH52P2q/hQEdLDj60H75G/m6fz2MneJU=', '/Uploads\\avatar_1.jpg', 2),
(5, 'Nombre', 'Apellido', 'hola@gmail.com', 'a8m9KfoAdA5bH52P2q/hQEdLDj60H75G/m6fz2MneJU=', '/Uploads\\avatar_5.jpeg', 1),
(7, 'a', 'a', 'a@a.com', 'a8m9KfoAdA5bH52P2q/hQEdLDj60H75G/m6fz2MneJU=', '/Uploads\\avatar_7.gif', 1),
(8, 'Lionel', 'Messi', 'tresestrellas@scaloneta.com', 'a8m9KfoAdA5bH52P2q/hQEdLDj60H75G/m6fz2MneJU=', '/Uploads\\avatar_8.jpg', 1),
(12, 'Juan Roman', 'Riquelme', '6ta@bianchi.com', 'a8m9KfoAdA5bH52P2q/hQEdLDj60H75G/m6fz2MneJU=', '/Uploads\\avatar_12.jpg', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id_contrato`),
  ADD KEY `id_inmueble` (`id_inmueble`),
  ADD KEY `id_inquilino` (`id_inquilino`);

--
-- Indexes for table `enum_tipos`
--
ALTER TABLE `enum_tipos`
  ADD PRIMARY KEY (`id_tipo`);

--
-- Indexes for table `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id_inmueble`),
  ADD KEY `id_propietario` (`id_propietario`);

--
-- Indexes for table `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id_inquilino`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indexes for table `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id_pago`),
  ADD KEY `id_contrato` (`id_contrato`);

--
-- Indexes for table `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_propietario`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indexes for table `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_usuario`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `email_2` (`email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `enum_tipos`
--
ALTER TABLE `enum_tipos`
  MODIFY `id_tipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`id_inmueble`) REFERENCES `inmuebles` (`id_inmueble`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilinos` (`id_inquilino`);

--
-- Constraints for table `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`id_propietario`) REFERENCES `propietarios` (`id_propietario`);

--
-- Constraints for table `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`id_contrato`) REFERENCES `contratos` (`id_contrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
