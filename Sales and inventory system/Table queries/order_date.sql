CREATE TABLE orders_history(
	history_id INT PRIMARY KEY IDENTITY(1,1),
	item_id INT NOT NULL FOREIGN KEY REFERENCES items(item_id),
	sales_id INT NOT NULL FOREIGN KEY REFERENCES sales_history(sales_id),
	date_ordered INT NULL FOREIGN KEY REFERENCES ordered_date(date_id),
	order_quantity INT NOT NULL,
	order_date DATE NOT NULL,
	total_cost INT NOT NULL,
	customer_name VARCHAR(255)
)