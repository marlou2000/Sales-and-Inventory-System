CREATE TABLE sales_history(
	sales_id INT PRIMARY KEY IDENTITY(1,1),
	total_item_quantity INT NOT NULL,
	total_cost INT NOT NULL,
	date_id INT NULL FOREIGN KEY REFERENCES date(date_id)
)