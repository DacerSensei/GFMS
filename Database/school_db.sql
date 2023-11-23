-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 24, 2023 at 12:09 AM
-- Server version: 10.4.25-MariaDB
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `school_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounting`
--

CREATE TABLE `accounting` (
  `id` int(11) NOT NULL,
  `registration_id` varchar(11) NOT NULL,
  `payment` text NOT NULL,
  `created_at` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `current_school_year`
--

CREATE TABLE `current_school_year` (
  `schlyr` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `current_school_year`
--

INSERT INTO `current_school_year` (`schlyr`) VALUES
('2023-2024');

-- --------------------------------------------------------

--
-- Table structure for table `notification`
--

CREATE TABLE `notification` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `message` varchar(255) NOT NULL,
  `status` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `previous_school`
--

CREATE TABLE `previous_school` (
  `id` int(11) NOT NULL,
  `student_id` int(11) NOT NULL,
  `school_name` text NOT NULL,
  `school_address` text NOT NULL,
  `school_mobile` varchar(20) NOT NULL,
  `guidance_name` varchar(100) NOT NULL,
  `guidance_mobile` varchar(20) NOT NULL,
  `principal_name` varchar(100) NOT NULL,
  `principal_mobile` varchar(20) NOT NULL,
  `adviser_name` varchar(100) NOT NULL,
  `adviser_mobile` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `registration`
--

CREATE TABLE `registration` (
  `id` int(11) NOT NULL,
  `student_id` int(11) NOT NULL,
  `registration_date` varchar(100) NOT NULL,
  `year` varchar(20) NOT NULL,
  `level` varchar(100) NOT NULL,
  `grade` varchar(20) NOT NULL,
  `pic` longblob NOT NULL,
  `status` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `school_profile`
--

CREATE TABLE `school_profile` (
  `school_id` varchar(50) NOT NULL,
  `school_name` text NOT NULL,
  `school_address` text NOT NULL,
  `school_contactno` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `school_profile`
--

INSERT INTO `school_profile` (`school_id`, `school_name`, `school_address`, `school_contactno`) VALUES
('424531', 'PALM VALLEY MULTIPLE INTELLIGENCE SCHOOL, INC', 'PALM VALLEY', '');

-- --------------------------------------------------------

--
-- Table structure for table `school_year`
--

CREATE TABLE `school_year` (
  `id` int(11) NOT NULL,
  `year` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `school_year`
--

INSERT INTO `school_year` (`id`, `year`) VALUES
(1, '2023-2024'),
(2, '2024-2025');

-- --------------------------------------------------------

--
-- Table structure for table `student`
--

CREATE TABLE `student` (
  `id` int(11) NOT NULL,
  `lrn` varchar(20) NOT NULL,
  `lastname` varchar(100) NOT NULL,
  `firstname` varchar(100) NOT NULL,
  `middlename` varchar(100) NOT NULL,
  `nickname` varchar(100) NOT NULL,
  `address` text NOT NULL,
  `birthplace` text NOT NULL,
  `gender` varchar(10) NOT NULL,
  `mobile` varchar(20) NOT NULL,
  `religion` text NOT NULL,
  `birthdate` varchar(100) NOT NULL,
  `citizenship` varchar(50) NOT NULL,
  `siblings` int(11) NOT NULL,
  `orderfamily` int(11) NOT NULL,
  `interest` text NOT NULL,
  `health` text NOT NULL,
  `father_name` varchar(100) NOT NULL,
  `father_work` varchar(100) NOT NULL,
  `father_mobile` varchar(20) NOT NULL,
  `mother_name` varchar(100) NOT NULL,
  `mother_work` varchar(100) NOT NULL,
  `mother_mobile` varchar(20) NOT NULL,
  `esc` varchar(100) DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `studentgrades`
--

CREATE TABLE `studentgrades` (
  `id` int(11) NOT NULL,
  `registration_id` int(11) NOT NULL,
  `student_level` varchar(255) NOT NULL,
  `subjects` text NOT NULL,
  `behavior` text NOT NULL,
  `attendance` text NOT NULL,
  `narrative` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='Test table for student''s grades';

-- --------------------------------------------------------

--
-- Table structure for table `student_requirements`
--

CREATE TABLE `student_requirements` (
  `id` int(11) NOT NULL,
  `student_id` int(11) NOT NULL,
  `path` text NOT NULL,
  `description` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `subjects`
--

CREATE TABLE `subjects` (
  `id` int(11) NOT NULL,
  `json` text NOT NULL,
  `type` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `subjects`
--

INSERT INTO `subjects` (`id`, `json`, `type`) VALUES
(1, '[{\"BehaviorDescription\":\"1. God-fearing - expresses one\'s spiritual beliefs while respecting the spiritual beliefs of others with strong adherence to ethical principles by upholding truth, integritiy and credibility at all times.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"2. Pro-Humanity - values individual differences and capabilities to bring about positive change and solidarity without any form of stereotyping nor prejudices.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"3. Pro-Environment - cares for the environment and utilizes resources wisely, judiciously and economically.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"4. Personal Discipline - demonstrates the will power to develop appropriate behavior in carrying out activities in the school and community.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"5. Life-long Learning - demonstrates love of truth, critical thinking and creativity in solving problems.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"6. Total Development - show progress in developing his/her God-given multiple intelligences by actively joining extra-curricular activities.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"7. Nationalistic - exercises his/her rights and responsibilities as a Filipino with appropriate display of conduct for the benefit of the majority.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"8. Institutional Discipline - demonstrates initiative to embrace PVMIS D.O.E.R (Disciplined, Organized, Excellent && Responsive to changes) attributes to his/her lifestyle.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"9. Social Discipline - demonstrates courtesy and restraint in conducting himself/herself in public or when dealing with other people in school and in the community.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"10. Occupational Discipline - demonstrates the will to improve his/her knowledge && skills in his/her chosen path.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null}]\n', 'BEHAVIOR'),
(2, '[{\"AttendanceDescription\":\"Number of School Days\",\"Aug\":\"\",\"Sept\":\"\",\"Oct\":\"\",\"Nov\":\"\",\"Dec\":\"\",\"Jan\":\"\",\"Feb\":\"\",\"Mar\":\"\",\"Apr\":\"\",\"May\":\"\",\"Jun\":\"\"},{\"AttendanceDescription\":\"Number of School Days Present\",\"Aug\":\"\",\"Sept\":\"\",\"Oct\":\"\",\"Nov\":\"\",\"Dec\":\"\",\"Jan\":\"\",\"Feb\":\"\",\"Mar\":\"\",\"Apr\":\"\",\"May\":\"\",\"Jun\":\"\"},{\"AttendanceDescription\":\"Number of times Tardy\",\"Aug\":\"\",\"Sept\":\"\",\"Oct\":\"\",\"Nov\":\"\",\"Dec\":\"\",\"Jan\":\"\",\"Feb\":\"\",\"Mar\":\"\",\"Apr\":\"\",\"May\":\"\",\"Jun\":\"\"}]', 'ATTENDANCE'),
(7, '[{\"Title\":\"First Quarter\",\"Message\":\"\",\"Signature\":\"\",\"Date\":\"\"},{\"Title\":\"Second Quarter\",\"Message\":\"\",\"Signature\":\"\",\"Date\":\"\"},{\"Title\":\"Third Quarter\",\"Message\":\"\",\"Signature\":\"\",\"Date\":\"\"},{\"Title\":\"Fourth Quarter\",\"Message\":\"\",\"Signature\":\"\",\"Date\":\"\"}]', 'NARRATIVE'),
(8, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'TOODLER'),
(14, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'NURSERY'),
(15, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'KINDER 1'),
(16, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'KINDER 2'),
(17, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 1'),
(18, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 2'),
(19, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 3'),
(20, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"HELE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 4'),
(21, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"HELE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 5'),
(22, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"HELE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 6'),
(23, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"TLE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 7'),
(24, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"TLE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 8'),
(25, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"TLE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 9'),
(26, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"TLE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 10'),
(27, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 11 - ABM'),
(28, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 11 - HUMSS'),
(29, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 11 - STEM'),
(30, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 11 - GAS'),
(31, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 12 - ABM'),
(32, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 12 - HUMSS'),
(33, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 12 - STEM'),
(34, '[{\"SubjectName\":\"Oral Communication\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Earth and Life Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Disciplines and Ideas in Social Sciences\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Philippine Politics and Governance\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"General Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Komunikasyon at Pananaliksik sa Wika at Kulturang Pilipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino sa Piling Larangan\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education & Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'Grade 12 - GAS');

-- --------------------------------------------------------

--
-- Table structure for table `sysadmin`
--

CREATE TABLE `sysadmin` (
  `userid` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `lastname` varchar(50) NOT NULL,
  `firstname` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `sysadmin`
--

INSERT INTO `sysadmin` (`userid`, `username`, `password`, `lastname`, `firstname`) VALUES
(1, 'admin', 'admin', 'admin', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `teachers`
--

CREATE TABLE `teachers` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `grade` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `teachers`
--

INSERT INTO `teachers` (`id`, `user_id`, `grade`) VALUES
(8, 32, 'TOODLER'),
(9, 36, 'TOODLER');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `lastname` varchar(100) NOT NULL,
  `firstname` varchar(100) NOT NULL,
  `email` varchar(255) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `usertype` varchar(30) NOT NULL,
  `status` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `lastname`, `firstname`, `email`, `username`, `password`, `usertype`, `status`) VALUES
(17, 'Admin', 'Admin', 'admin@gmail.com', 'admin', 'admin', 'ADMIN', 1),
(32, 'orijuela', 'dave', 'dacerz14@gmail.com', 'a', 'a', 'TEACHER', 1),
(33, 'Orijuela', 'Dave', 'dave@gmail.com', 'principal', '123', 'PRINCIPAL', 1),
(34, 'Orijuela', 'Dave', 'finance@yahoo.com', 'finance', '123', 'FINANCE', 1),
(35, 'Orijuela', 'Dave', 'registrar@yahoo.com', 'registrar', '123', 'REGISTRAR', 1),
(36, 'Orijuela', 'Dave', 'teacher@gmail.com', 'teacher', '123', 'TEACHER', 1),
(37, 'Jackson', 'Michael', 'hello@yahoo.com', 'registrar1', '123', 'REGISTRAR', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounting`
--
ALTER TABLE `accounting`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `notification`
--
ALTER TABLE `notification`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `previous_school`
--
ALTER TABLE `previous_school`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `registration`
--
ALTER TABLE `registration`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `school_year`
--
ALTER TABLE `school_year`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `student`
--
ALTER TABLE `student`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `studentgrades`
--
ALTER TABLE `studentgrades`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `student_requirements`
--
ALTER TABLE `student_requirements`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `subjects`
--
ALTER TABLE `subjects`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysadmin`
--
ALTER TABLE `sysadmin`
  ADD PRIMARY KEY (`userid`);

--
-- Indexes for table `teachers`
--
ALTER TABLE `teachers`
  ADD PRIMARY KEY (`id`),
  ADD KEY `teachers_FK_1` (`user_id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounting`
--
ALTER TABLE `accounting`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `notification`
--
ALTER TABLE `notification`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `previous_school`
--
ALTER TABLE `previous_school`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `registration`
--
ALTER TABLE `registration`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `school_year`
--
ALTER TABLE `school_year`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `student`
--
ALTER TABLE `student`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `studentgrades`
--
ALTER TABLE `studentgrades`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `student_requirements`
--
ALTER TABLE `student_requirements`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=76;

--
-- AUTO_INCREMENT for table `subjects`
--
ALTER TABLE `subjects`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT for table `sysadmin`
--
ALTER TABLE `sysadmin`
  MODIFY `userid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `teachers`
--
ALTER TABLE `teachers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `teachers`
--
ALTER TABLE `teachers`
  ADD CONSTRAINT `teachers_FK_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
