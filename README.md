To run the project =====>
->Add mysql database on localhost
->Name it `temp`
->Create a new table by running following script :

CREATE TABLE `tbl_users` (
  `UserName` text NOT NULL,
  `Badge` varchar(15) NOT NULL,
  `EnrollNo` varchar(15) NOT NULL,
  `Department` varchar(15) NOT NULL,
  `Email` text NOT NULL,
  `Password` text NOT NULL,
  `Photo` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

Now you're good to go. 
