--1. Query all of the entries in the Genre table
SELECT * FROM Genre;

--2. Using the INSERT statement, add one of your favorite artists to the Artist table.
INSERT INTO Artist (ArtistName, YearEstablished)
VALUES ('DAWES', '2009');

--3. Using the INSERT statement, add one, or more, albums by your artist to the Album table.
INSERT INTO Album (Title, ReleaseDate, AlbumLength, Label, ArtistId, GenreId)
VALUES ('Stories Dont End', '4/9/2013', 3070, 'HUB', 28, 2);

--4. Using the INSERT statement, add some songs that are on that album to the Song table.
INSERT INTO Song (Title, SongLength, ReleaseDate, GenreId, ArtistId, AlbumId)
VALUES ('Just Beneath the Surface', 249, '4/9/2013', 2, 28, 23), 
('From a Window Seat', 268, '4/9/2013', 2, 28, 23);

--5. Write a SELECT query that provides the song titles, album title, and artist name for all of the data you just entered in.
--Use the LEFT JOIN keyword sequence to connect the tables, and the WHERE keyword to filter the results to the album and artist you added.
SELECT a.Title, s.Title, art.ArtistName FROM Album a
LEFT JOIN Song s ON s.Albumid = a.id
LEFT JOIN Artist art ON art.id = a.ArtistId
WHERE a.ArtistId = 28;

--6. Write a SELECT statement to display how many songs exist for each album. You'll need to use the COUNT() function and the GROUP BY keyword sequence.
SELECT a.Title, COUNT(s.Title) AS 'Number of Songs' FROM Album a
LEFT JOIN Song s ON s.AlbumId = a.Id
GROUP BY a.Title;

--7. Write a SELECT statement to display how many songs exist for each artist. You'll need to use the COUNT() function and the GROUP BY keyword sequence.
SELECT a.ArtistName, COUNT(s.Title) AS 'Number of Songs' FROM Artist a
LEFT JOIN Song s ON s.ArtistId = a.Id
GROUP BY a.ArtistName;

--8. Write a SELECT statement to display how many songs exist for each genre. You'll need to use the COUNT() function and the GROUP BY keyword sequence.
SELECT g.Label, COUNT(s.Title) AS 'Number of Songs' FROM Genre g
LEFT JOIN Song s ON s.GenreId = g.Id
GROUP BY g.Label;

--9. Using MAX() function, write a select statement to find the album with the longest duration. The result should display the album title and the duration.
SELECT a.Title, a.AlbumLength AS 'Max Length' FROM Album a
WHERE a.AlbumLength = (SELECT MAX(a.AlbumLength) FROM Album a);

--10. Using MAX() function, write a select statement to find the song with the longest duration. The result should display the song title and the duration.
SELECT s.Title, s.SongLength AS 'Max Length' FROM Song s
WHERE s.SongLength = (SELECT MAX(s.SongLength) FROM Song s);

--11. Modify the previous query to also display the title of the album.
SELECT a.Title AS 'Album', s.Title AS 'Song', s.SongLength AS 'Max Length' FROM Song s
JOIN Album a ON a.Id = s.AlbumId
WHERE s.SongLength = (SELECT MAX(s.SongLength) FROM Song s);




