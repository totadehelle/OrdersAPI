#fills the orders database with 6 000 000 entries for testing

i=1
for j in {1..10}; do
	echo "insert into orders (Id, OrderId, CustomerName, Phone, Email, Address, DeliveryDate, Comment, OfferAccepted, OrderIp) 	
	VALUES 	($i, $i, 'Joe0','phone0','email0','address0', '2/20/19 12:18:49 PM', 'comment0', true, '0.0.0.0')" 
	> query.sql	
	min=$((i+1))
	max=$((600000*j))	
	for x in $(seq $min $max); 
		do
			n=$((x%4))	
			echo ",($x, $x,'Joe$n','phone$n','email$n','address$n', '2/2$n/19 12:18:49 PM', 'comment$n', true, '0.0.0.$n')" 
			>> query.sql
		done;
	i=$((i+600000*j))
	echo 'done'
	echo ';' >> query.sql		
	q=$(cat query.sql)
	echo $q | sqlite3 OrdersDatabase.db 
	echo $j / 10	
done;