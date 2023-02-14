CREATE TABLE date(
	date_id INT PRIMARY KEY IDENTITY(1,1),
	date_ordered DATE NOT NULL,
	year_ordered INT NOT NULL,
	month_ordered INT NOT NULL,
	week_ordered INT NOT NULL,
	week_range_month VARCHAR(255) NOT NULL,
	week_range_day_of_the_week VARCHAR(255) NOT NULL,
	day_ordered INT NOT NULL,
	day_of_the_week_ordered VARCHAR(255) NOT NULL
)