CREATE TABLE orders_history(
	history_id INT PRIMARY KEY IDENTITY(1,1),
	item_serial_number INT NOT NULL FOREIGN KEY REFERENCES item(item_serial_number),
	ordered_quantity INT NOT NULL,
	total_cost_per_item INT NOT NULL,
	customer_name VARCHAR(255),
	sales_id INT NOT NULL FOREIGN KEY REFERENCES sales_history(sales_id)
)