-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 03, 2023 at 11:04 PM
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
  `accounting_id` int(11) NOT NULL,
  `registration_id` int(11) NOT NULL,
  `payment` int(11) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
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

--
-- Dumping data for table `previous_school`
--

INSERT INTO `previous_school` (`id`, `student_id`, `school_name`, `school_address`, `school_mobile`, `guidance_name`, `guidance_mobile`, `principal_name`, `principal_mobile`, `adviser_name`, `adviser_mobile`) VALUES
(51, 74, 'New', 'None', '123', 'None', '123', 'None', '123', 'None', '123'),
(52, 75, '123', '123', '123', '123', '123', '123', '123', '123', '123'),
(53, 76, '123', '123123', '123', '123', '12312', '123', '3123', '123', '123123');

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

--
-- Dumping data for table `registration`
--

INSERT INTO `registration` (`id`, `student_id`, `registration_date`, `year`, `level`, `grade`, `pic`, `status`) VALUES
INSERT INTO `registration` (`id`, `student_id`, `registration_date`, `year`, `level`, `grade`, `pic`, `status`) VALUES
INSERT INTO `registration` (`id`, `student_id`, `registration_date`, `year`, `level`, `grade`, `pic`, `status`) VALUES

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
  `mother_mobile` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `student`
--

INSERT INTO `student` (`id`, `lrn`, `lastname`, `firstname`, `middlename`, `nickname`, `address`, `birthplace`, `gender`, `mobile`, `religion`, `birthdate`, `citizenship`, `siblings`, `orderfamily`, `interest`, `health`, `father_name`, `father_work`, `father_mobile`, `mother_name`, `mother_work`, `mother_mobile`) VALUES
(74, '22222222222', 'Orijuela', 'Dave', 'Catan', 'Dave', 'Montalban', 'Quezon', 'MALE', '09666232523', 'INC', '06/02/2000', 'Filipino', 2, 1, 'Programming', 'None', 'None', 'None', '123', 'None', 'None', '123'),
(75, '12312321312', '123123', '1231', '23', '123', '123', '123', 'MALE', '123', '123', '11/02/2023', '123', 12, 12, '123', '123', '123', '123', '123', '123', '123', '123'),
(76, '12', '3123', '123', '123', '123', '123', '123', 'MALE', '123', '123', '11/02/2023', '123', 12, 12, '123', '1231', '123', '123', '123', '123', '123', '123');

-- --------------------------------------------------------

--
-- Table structure for table `studentgrades`
--

CREATE TABLE `studentgrades` (
  `id` int(11) NOT NULL,
  `registration_id` int(11) NOT NULL,
  `student_level` varchar(255) NOT NULL,
  `data` text NOT NULL
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
  `subjects` text NOT NULL,
  `grade` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `subjects`
--

INSERT INTO `subjects` (`id`, `subjects`, `grade`) VALUES
(1, '[{\"BehaviorDescription\":\"1. God-fearing - expresses one\'s spiritual beliefs while respecting the spiritual beliefs of others with strong adherence to ethical principles by upholding truth, integritiy and credibility at all times.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"2. Pro-Humanity - values individual differences and capabilities to bring about positive change and solidarity without any form of stereotyping nor prejudices.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"3. Pro-Environment - cares for the environment and utilizes resources wisely, judiciously and economically.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"4. Personal Discipline - demonstrates the will power to develop appropriate behavior in carrying out activities in the school and community.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"5. Life-long Learning - demonstrates love of truth, critical thinking and creativity in solving problems.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"6. Total Development - show progress in developing his/her God-given multiple intelligences by actively joining extra-curricular activities.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"7. Nationalistic - exercises his/her rights and responsibilities as a Filipino with appropriate display of conduct for the benefit of the majority.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"8. Institutional Discipline - demonstrates initiative to embrace PVMIS D.O.E.R (Disciplined, Organized, Excellent && Responsive to changes) attributes to his/her lifestyle.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"9. Social Discipline - demonstrates courtesy and restraint in conducting himself/herself in public or when dealing with other people in school and in the community.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null},{\"BehaviorDescription\":\"10. Occupational Discipline - demonstrates the will to improve his/her knowledge && skills in his/her chosen path.\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null}]\n', 'BEHAVIOR'),
(2, '[{\"AttendanceDescription\":\"Number of School Days\",\"Aug\":\"\",\"Sept\":\"\",\"Oct\":\"\",\"Nov\":\"\",\"Dec\":\"\",\"Jan\":\"\",\"Feb\":\"\",\"Mar\":\"\",\"Apr\":\"\",\"May\":\"\",\"Jun\":\"\"},{\"AttendanceDescription\":\"Number of School Days Present\",\"Aug\":\"\",\"Sept\":\"\",\"Oct\":\"\",\"Nov\":\"\",\"Dec\":\"\",\"Jan\":\"\",\"Feb\":\"\",\"Mar\":\"\",\"Apr\":\"\",\"May\":\"\",\"Jun\":\"\"},{\"AttendanceDescription\":\"Number of times Tardy\",\"Aug\":\"\",\"Sept\":\"\",\"Oct\":\"\",\"Nov\":\"\",\"Dec\":\"\",\"Jan\":\"\",\"Feb\":\"\",\"Mar\":\"\",\"Apr\":\"\",\"May\":\"\",\"Jun\":\"\"}]\r\n', 'ATTENDANCE'),
(3, '[{\"SubjectName\":\"English\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Filipino\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Mathematics\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Science\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Social Studies / AP\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Reading and Writing\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Character Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"HELE / ICT\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"ESL\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Music\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Arts\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Physical Education\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null},{\"SubjectName\":\"Health\",\"FirstGrading\":null,\"SecondGrading\":null,\"ThirdGrading\":null,\"FourthGrading\":null,\"FinalRating\":null,\"Remarks\":null}]', 'TOODLER'),
(5, '[{\"AttendanceDescription\":\"Number of School Days\",\"Aug\":\"32\",\"Sept\":\"45\",\"Oct\":\"18\",\"Nov\":\"74\",\"Dec\":\"23\",\"Jan\":\"57\",\"Feb\":\"91\",\"Mar\":\"65\",\"Apr\":\"89\",\"May\":\"42\",\"Jun\":\"76\"},{\"AttendanceDescription\":\"Number of School Days Present\",\"Aug\":\"19\",\"Sept\":\"53\",\"Oct\":\"37\",\"Nov\":\"68\",\"Dec\":\"25\",\"Jan\":\"84\",\"Feb\":\"49\",\"Mar\":\"62\",\"Apr\":\"12\",\"May\":\"70\",\"Jun\":\"36\"},{\"AttendanceDescription\":\"Number of times Tardy\",\"Aug\":\"47\",\"Sept\":\"28\",\"Oct\":\"95\",\"Nov\":\"60\",\"Dec\":\"31\",\"Jan\":\"73\",\"Feb\":\"56\",\"Mar\":\"14\",\"Apr\":\"29\",\"May\":\"80\",\"Jun\":\"51\"}]', 'ATTENDANCE'),
(6, '[{\"BehaviorDescription\":\"1. God-fearing - expresses one\'s spiritual beliefs while respecting the spiritual beliefs of others with strong adherence to ethical principles by upholding truth, integritiy and credibility at all times.\",\"FirstGrading\":\"AC\",\"SecondGrading\":\"GK\",\"ThirdGrading\":\"RT\",\"FourthGrading\":\"QW\"},{\"BehaviorDescription\":\"2. Pro-Humanity - values individual differences and capabilities to bring about positive change and solidarity without any form of stereotyping nor prejudices.\",\"FirstGrading\":\"IO\",\"SecondGrading\":\"DF\",\"ThirdGrading\":\"LH\",\"FourthGrading\":\"MP\"},{\"BehaviorDescription\":\"3. Pro-Environment - cares for the environment and utilizes resources wisely, judiciously and economically.\",\"FirstGrading\":\"VY\",\"SecondGrading\":\"BZ\",\"ThirdGrading\":\"KX\",\"FourthGrading\":\"NE\"},{\"BehaviorDescription\":\"4. Personal Discipline - demonstrates the will power to develop appropriate behavior in carrying out activities in the school and community.\",\"FirstGrading\":\"US\",\"SecondGrading\":\"RW\",\"ThirdGrading\":\"PT\",\"FourthGrading\":\"OQ\"},{\"BehaviorDescription\":\"5. Life-long Learning - demonstrates love of truth, critical thinking and creativity in solving problems.\",\"FirstGrading\":\"FJ\",\"SecondGrading\":\"LK\",\"ThirdGrading\":\"AB\",\"FourthGrading\":\"CD\"},{\"BehaviorDescription\":\"6. Total Development - show progress in developing his/her God-given multiple intelligences by actively joining extra-curricular activities.\",\"FirstGrading\":\"EF\",\"SecondGrading\":\"MN\",\"ThirdGrading\":\"GH\",\"FourthGrading\":\"IJ\"},{\"BehaviorDescription\":\"7. Nationalistic - exercises his/her rights and responsibilities as a Filipino with appropriate display of conduct for the benefit of the majority.\",\"FirstGrading\":\"DE\",\"SecondGrading\":\"YZ\",\"ThirdGrading\":\"KL\",\"FourthGrading\":\"OP\"},{\"BehaviorDescription\":\"8. Institutional Discipline - demonstrates initiative to embrace PVMIS D.O.E.R (Disciplined, Organized, Excellent && Responsive to changes) attributes to his/her lifestyle.\",\"FirstGrading\":\"ST\",\"SecondGrading\":\"UV\",\"ThirdGrading\":\"WX\",\"FourthGrading\":\"GH\"},{\"BehaviorDescription\":\"9. Social Discipline - demonstrates courtesy and restraint in conducting himself/herself in public or when dealing with other people in school and in the community.\",\"FirstGrading\":\"IJ\",\"SecondGrading\":\"KL\",\"ThirdGrading\":\"OP\",\"FourthGrading\":\"BC\"},{\"BehaviorDescription\":\"10. Occupational Discipline - demonstrates the will to improve his/her knowledge && skills in his/her chosen path.\",\"FirstGrading\":\"NO\",\"SecondGrading\":\"XY\",\"ThirdGrading\":\"UV\",\"FourthGrading\":\"IJ\"}]', 'BEHAVIOR');

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
(5, 25, 'TOODLER'),
(6, 27, 'TOODLER');

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
(17, 'Orijuela', 'Dave', 'dacerz14@gmail.com', 'b', 'b', 'REGISTRAR', 1),
(22, 'e', 'e', 'Finance@gmail.com', 'e', 'e', 'FINANCE', 1),
(25, 'g', 'g', 'g', 'g', 'g', 'TEACHER', 1),
(26, 'Orijuela', 'Dave', 'michaeljaksun@gmail.com', 'admin', 'admin', 'ADMIN', 1),
(27, 'Test', 'Sample', 'teacher@gmail.com', 'a', 'a', 'TEACHER', 1);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_student`
-- (See below for the actual view)
--
CREATE TABLE `vw_student` (
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_student_registration`
-- (See below for the actual view)
--
CREATE TABLE `vw_student_registration` (
);

-- --------------------------------------------------------

--
-- Structure for view `vw_student`
--
DROP TABLE IF EXISTS `vw_student`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_student`  AS SELECT `student`.`student_id` AS `student_id`, `student`.`lrn` AS `lrn`, `student`.`lastname` AS `lastname`, `student`.`firstname` AS `firstname`, `student`.`middlename` AS `middlename`, concat(`student`.`lastname`,', ',`student`.`firstname`,' ',`student`.`middlename`) AS `student_name`, `student`.`nickname` AS `nickname`, `student`.`address` AS `address`, `student`.`birthdate` AS `birthdate`, `student`.`birthplace` AS `birthplace`, `student`.`gender` AS `gender`, `student`.`mobileno` AS `mobileno`, `student`.`religion` AS `religion`, `student`.`father_name` AS `father_name`, `student`.`mother_name` AS `mother_name` FROM `student``student`  ;

-- --------------------------------------------------------

--
-- Structure for view `vw_student_registration`
--
DROP TABLE IF EXISTS `vw_student_registration`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_student_registration`  AS SELECT `t1`.`student_id` AS `student_id`, `t2`.`registration_id` AS `registration_id`, `t3`.`prevschool_id` AS `prevschool_id`, `t1`.`lrn` AS `lrn`, `t1`.`lastname` AS `lastname`, `t1`.`firstname` AS `firstname`, `t1`.`middlename` AS `middlename`, concat(`t1`.`lastname`,', ',`t1`.`firstname`,' ',`t1`.`middlename`) AS `student_name`, `t1`.`nickname` AS `nickname`, `t1`.`citizenship` AS `citizenship`, `t1`.`no_of_siblings` AS `noofsiblings`, `t1`.`health_condition` AS `healthcondition`, `t1`.`order_in_family` AS `orderinfamily`, `t1`.`address` AS `address`, `t1`.`birthdate` AS `birthdate`, `t1`.`birthplace` AS `birthplace`, `t1`.`major_interests` AS `major_interests`, `t1`.`gender` AS `gender`, `t1`.`mobileno` AS `mobileno`, `t1`.`religion` AS `religion`, `t1`.`father_name` AS `father_name`, `t1`.`father_work` AS `father_work`, `t1`.`father_contact_no` AS `father_contact_no`, `t1`.`mother_name` AS `mother_name`, `t1`.`mother_work` AS `mother_work`, `t1`.`mother_contact_no` AS `mother_contact_no`, `t2`.`schlyr` AS `schlyr`, `t2`.`grade` AS `grade`, `t2`.`pic` AS `pic`, `t3`.`school_name` AS `school_name`, `t3`.`school_address` AS `school_address`, `t3`.`school_contactno` AS `school_contactno`, `t3`.`guidance_name` AS `guidance_name`, `t3`.`guidance_contactno` AS `guidance_contactno`, `t3`.`principal_name` AS `principal_name`, `t3`.`principal_contactno` AS `principal_contactno`, `t3`.`adviser_name` AS `adviser_name`, `t3`.`adviser_contactno` AS `adviser_contactno`, `t2`.`registration_status` AS `registration_status` FROM ((`student` `t1` left join `registration` `t2` on(`t1`.`student_id` = `t2`.`student_id`)) left join `previous_school` `t3` on(`t2`.`registration_id` = `t3`.`registration_id`))  ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounting`
--
ALTER TABLE `accounting`
  ADD PRIMARY KEY (`accounting_id`);

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
  MODIFY `accounting_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `previous_school`
--
ALTER TABLE `previous_school`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT for table `registration`
--
ALTER TABLE `registration`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=49;

--
-- AUTO_INCREMENT for table `school_year`
--
ALTER TABLE `school_year`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `student`
--
ALTER TABLE `student`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=77;

--
-- AUTO_INCREMENT for table `studentgrades`
--
ALTER TABLE `studentgrades`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `student_requirements`
--
ALTER TABLE `student_requirements`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=63;

--
-- AUTO_INCREMENT for table `subjects`
--
ALTER TABLE `subjects`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `sysadmin`
--
ALTER TABLE `sysadmin`
  MODIFY `userid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `teachers`
--
ALTER TABLE `teachers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

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