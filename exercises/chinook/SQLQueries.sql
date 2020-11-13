---1) Provide a query showing Customers (just their full names, customer ID and country) who are not in the US.

SELECT CustomerId, FirstName, LastName, Country 
	FROM Customer
	WHERE Country != 'USA';

--- 2) Provide a query only showing the Customers from Brazil.

SELECT CustomerId, FirstName, LastName, Country 
	FROM Customer
	WHERE Country = 'Brazil';

--- 3) Provide a query showing the Invoices of customers who are from Brazil. The resultant table should show the customer's 
--- full name, Invoice ID, Date of the invoice and billing country.

SELECT c.FirstName, c.LastName, i.InvoiceId, i.InvoiceDate, i.BillingCountry
	FROM Invoice i
	LEFT JOIN Customer c ON i.CustomerId = c.CustomerId
	WHERE c.Country = 'Brazil';

--- 4) Provide a query showing only the Employees who are Sales Agents.

SELECT *
	FROM Employee
	WHERE Title = 'Sales Support Agent';

--- 5) Provide a query showing a unique/distinct list of billing countries from the Invoice table.

SELECT DISTINCT BillingCountry
	FROM Invoice;

--- 6) Provide a query that shows the invoices associated with each sales agent. The resultant table should include the Sales Agent's full name.

SELECT e.FirstName, e.LastName, i.*
	FROM Employee e
	LEFT JOIN Customer c ON e.EmployeeId = c.SupportRepId
	LEFT JOIN Invoice i ON c.CustomerId = i.CustomerId
	WHERE e.Title = 'Sales Support Agent';

--- 7) Provide a query that shows the Invoice Total, Customer name, Country and Sale Agent name for all invoices and customers.

SELECT i.Total, c.FirstName AS CustomerFirstName, c.LastName AS CustomerLastName, c.Country, e.FirstName AS AgentFirstName, e.LastName AS AgentLastName
	FROM Invoice i
	LEFT JOIN Customer c ON i.CustomerId = c.CustomerId
	LEFT JOIN Employee e ON c.SupportRepId = e.EmployeeId
	WHERE e.Title = 'Sales Support Agent';

--- 8)  How many Invoices were there in 2009 and 2011?

SELECT COUNT(*) AS TotalInvoices
	FROM Invoice
	WHERE InvoiceDate BETWEEN '2009-01-01' AND '2009-12-31'
	OR InvoiceDate BETWEEN '2011-01-01' AND '2011-12-31';

--- 9) What are the respective total sales for each of those years?

SELECT SUM(Total) AS TotalSales2009
	FROM Invoice
	WHERE InvoiceDate BETWEEN '2009-01-01' AND '2009-12-31';

SELECT SUM(Total) AS TotalSales2011
	FROM Invoice
	WHERE InvoiceDate BETWEEN '2011-01-01' AND '2011-12-31';

--- 10) Looking at the InvoiceLine table, provide a query that COUNTs the number of line items for Invoice ID 37.

SELECT COUNT(*) AS InvoiceLineCount
	FROM Invoice i
	LEFT JOIN InvoiceLine il ON i.InvoiceId = il.InvoiceId
	WHERE i.InvoiceId = 37;

--- 11) Looking at the InvoiceLine table, provide a query that COUNTs the number of line items for each Invoice. HINT: GROUP BY

SELECT i.InvoiceId, COUNT(*) AS InvoiceLineCount
	FROM Invoice i
	LEFT JOIN InvoiceLine il ON i.InvoiceId = il.InvoiceId
	GROUP BY i.InvoiceId;

--- 12) Provide a query that includes the purchased track name with each invoice line item.

SELECT il.InvoiceLineId, t.Name
	FROM InvoiceLine il
	LEFT JOIN Track t ON il.TrackId = t.TrackId
	ORDER BY il.InvoiceLineId;

--- 13) Provide a query that includes the purchased track name AND artist name with each invoice line item.

SELECT il.InvoiceLineId, t.Name AS TrackName, ar.Name AS ArtistName
	FROM InvoiceLine il
	LEFT JOIN Track t ON il.TrackId = t.TrackId
	LEFT JOIN Album al ON t.AlbumId = al.AlbumId
	LEFT JOIN Artist ar ON al.ArtistId = ar.ArtistId
	ORDER BY il.InvoiceLineId;

--- 14) Provide a query that shows the # of invoices per country. HINT: GROUP BY

SELECT BillingCountry, Count(*) AS NumberInvoices
	FROM Invoice
	GROUP BY BillingCountry;

--- 15) Provide a query that shows the total number of tracks in each playlist. The Playlist name should be include on the resulant table.

SELECT p.Name AS PlaylistName, COUNT(*) AS NumberTracks
	FROM Playlist p
	LEFT JOIN PlaylistTrack pt ON p.PlaylistId = pt.PlaylistId
	LEFT JOIN Track t ON pt.TrackId = t.TrackId
	GROUP BY p.Name
	ORDER BY p.Name;

--- 16) Provide a query that shows all the Tracks, but displays no IDs. The result should include the Album name, Media type and Genre.

SELECT t.Name AS Track, a.Title AS Album, m.Name AS MediaType, g.Name AS Genre
	FROM Track t
	LEFT JOIN Album a ON t.AlbumId = a.AlbumId
	LEFT JOIN MediaType m ON t.MediaTypeId = m.MediaTypeId
	LEFT JOIN Genre g ON t.GenreId = g.GenreId;

--- 17) Provide a query that shows all Invoices but includes the # of invoice line items.

SELECT i.InvoiceId, COUNT(il.InvoiceId) AS InvoiceLines
	FROM Invoice i
	LEFT JOIN InvoiceLine il ON i.InvoiceId = il.InvoiceId
	GROUP BY i.InvoiceId;

--- 18) Provide a query that shows total sales made by each sales agent.

SELECT e.FirstName, e.LastName, SUM(i.Total) AS TotalSales
	FROM Employee e
	LEFT JOIN Customer c ON e.EmployeeId = c.SupportRepId
	LEFT JOIN Invoice i ON c.CustomerId = i.CustomerId
	WHERE e.Title = 'Sales Support Agent'
	GROUP BY e.FirstName, e.LastName;

--- 19) Which sales agent made the most in sales in 2009?

SELECT e.FirstName, e.LastName, SUM(i.Total) AS Total2019Sales
	FROM Employee e
	LEFT JOIN Customer c ON e.EmployeeId = c.SupportRepId
	LEFT JOIN Invoice i ON c.CustomerId = i.CustomerId
	WHERE i.InvoiceDate BETWEEN '2009-01-01' AND '2009-12-31'
	GROUP BY e.FirstName, e.LastName
	HAVING SUM(i.Total) = (SELECT MAX(Total2019Sales) FROM (SELECT SUM(i.Total) AS Total2019Sales
		FROM Employee e
		LEFT JOIN Customer c ON e.EmployeeId = c.SupportRepId
		LEFT JOIN Invoice i ON c.CustomerId = i.CustomerId
		WHERE i.InvoiceDate BETWEEN '2009-01-01' AND '2009-12-31'
		GROUP BY e.EmployeeId) AS a);

--- 20) Which sales agent made the most in sales over all?

SELECT e.FirstName, e.LastName, SUM(i.Total) AS TotalSales
	FROM Employee e
	LEFT JOIN Customer c ON e.EmployeeId = c.SupportRepId
	LEFT JOIN Invoice i ON c.CustomerId = i.CustomerId
	GROUP BY e.FirstName, e.LastName
	HAVING SUM(i.Total) = (SELECT MAX(TotalSales) FROM (SELECT SUM(i.Total) AS TotalSales
		FROM Employee e
		LEFT JOIN Customer c ON e.EmployeeId = c.SupportRepId
		LEFT JOIN Invoice i ON c.CustomerId = i.CustomerId
		GROUP BY e.EmployeeId) AS a);

--- 21) Provide a query that shows the count of customers assigned to each sales agent.

SELECT e.FirstName, e.LastName, COUNT(c.CustomerId) AS CustomerCount
	FROM Employee e
	LEFT JOIN Customer c ON e.EmployeeId = c.SupportRepId
	WHERE e.Title = 'Sales Support Agent'
	GROUP BY e.FirstName, e.LastName;

--- 22) Provide a query that shows the total sales per country.

SELECT i.BillingCountry, SUM(i.Total) AS TotalSales
	FROM Invoice i
	GROUP BY i.BillingCountry;

--- 23) Which country's customers spent the most?

SELECT i.BillingCountry, SUM(i.Total) AS TotalSales
	FROM Invoice i
	GROUP BY i.BillingCountry
	HAVING SUM(i.Total) = (SELECT MAX(TotalSales) 
		FROM (SELECT SUM(i.Total) AS TotalSales
			FROM Invoice i
			GROUP BY i.BillingCountry)AS a);

--- 24) Provide a query that shows the most purchased track of 2013.

SELECT t.Name AS TrackName, COUNT(il.InvoiceLineId) AS TimesPurchased
	FROM Track t
	LEFT JOIN InvoiceLine il ON il.TrackId = t.TrackId
	LEFT JOIN Invoice i ON il.InvoiceId = i.InvoiceId
	WHERE i.InvoiceDate BETWEEN '2013-01-01' AND '2013-12-13'
	GROUP BY t.Name
	HAVING COUNT(il.InvoiceLineId) = (SELECT MAX(TimesPurchased)
		FROM (SELECT COUNT(il.InvoiceLineId) AS TimesPurchased
			FROM Track t
			LEFT JOIN InvoiceLine il ON il.TrackId = t.TrackId
			LEFT JOIN Invoice i ON il.InvoiceId = i.InvoiceId
			WHERE i.InvoiceDate BETWEEN '2013-01-01' AND '2013-12-13'
			GROUP BY t.Name)AS a);

--- 25) Provide a query that shows the top 5 most purchased songs.

SELECT TOP(5) t.Name, COUNT(il.InvoiceLineId) AS TimesPurchased
	FROM Track t
	LEFT JOIN InvoiceLine il ON il.TrackId = t.TrackId
	GROUP BY t.Name
	ORDER BY TimesPurchased DESC;

--- 26) Provide a query that shows the top 3 best selling artists.

SELECT TOP(3) a.Name, COUNT(il.InvoiceLineId) AS TimesPurchased
	FROM Artist a
	LEFT JOIN Album al ON a.ArtistId = al.ArtistId
	LEFT JOIN Track t ON al.AlbumId = t.AlbumId
	LEFT JOIN InvoiceLine il ON t.TrackId = il.TrackId
	GROUP BY a.Name
	ORDER BY TimesPurchased DESC;

--- 27) Provide a query that shows the most purchased Media Type.

SELECT TOP(1) mt.Name, COUNT(il.InvoiceLineId) AS TimesPurchased
	FROM MediaType mt
	LEFT JOIN Track t ON mt.MediaTypeId = t.MediaTypeId
	LEFT JOIN InvoiceLine il ON t.TrackId = il.TrackId
	GROUP BY mt.Name
	ORDER BY TimesPurchased DESC;