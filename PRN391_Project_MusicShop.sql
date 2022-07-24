create database PRN391_Project_MusicShop
go 
use PRN391_Project_MusicShop
go
create table Accounts(
	accountId int identity(1,1) Primary Key,
	username nvarchar(20) not null,
	pword nvarchar(20) not null,
	role int not null
)
go
insert into Accounts(username, pword, role) values('dung151', '1234', 1)
go
create table Genres (
	GenreId int identity(1,1) PRIMARY KEY,
	Name nvarchar(50) NOT NULL,
	Description nvarchar(max) NOT NULL,
)
go
insert into Genres (Name, Description) values
('Pop', 'A genre of popular music that originated in the West during the 1950s and 1960s. Pop music is eclectic, often borrowing elements from urban, dance, rock, Latin, country, and other styles. Songs are typically short to medium-length with repeated choruses, melodic tunes, and hooks.'),
('Hip-hop','Hip hop or rap music formed in the United States in the 1970s and consists of stylized rhythmic music that commonly accompanies rhythmic and rhyming speech ("rapping").'),
('Rock','A genre of popular music that originated as "rock and roll" in the United States in the 1950s, and developed into a range of different styles in the 1960s and later. Compared to pop music, rock places a higher degree of emphasis on musicianship, live performance, and an ideology of authenticity.'),
('R&B','A genre of popular African-American music that originated in the 1940s as urbane, rocking, jazz based music with a heavy, insistent beat. Lyrics focus heavily on the themes of triumphs and failures in terms of relationships, freedom, economics, aspirations, and sex.'),
('Soul','A popular music genre that combines elements of African-American gospel music, rhythm and blues and jazz.'),
('Folk','A genre that evolved from traditional music during the 20th century folk revival. One meaning often given is that of old songs with no known composers; another is music that has been transmitted and evolved by a process of oral transmission or performed by custom over a long period of time.'),
('Jazz','A music genre that originated from African American communities of New Orleans during the late 19th and early 20th centuries in the form of independent traditional and popular musical styles, all linked by the common bonds of African American and European American musical parentage with a performance orientation.'),
('Disco','A genre of dance music containing elements of funk, soul, pop, and salsa that achieved popularity during the mid-1970s to the early 1980s.'),
('Classic','Art music produced or rooted in the traditions of Western music, including both liturgical and secular music, over the broad span of time from roughly the 11th century to the present day.'),
('Electronic','A large set of predominantly popular and dance genres in which synthesizers and other electronic instruments are the primary sources of sound.')
go
create table Artists (
	ArtistId int identity(1,1) PRIMARY KEY,
	Name nvarchar(100) NOT NULL,
	img nvarchar(max) null
)
go
insert into Artists values
('Harry Styles', null),
('Drake', null),
('Taylor Swift', null),
('The Weekend', null),
('Ed Sheeran', null),
('Maroon 5', null),
('Billie Eilish', null),
('Son Tung MTP', null),
('Vu.', null)
go
create table Albums(
	AlbumId int identity(1,1) PRIMARY KEY,
	GenreId int FOREIGN KEY REFERENCES Genres(GenreId),
	ArtistId int FOREIGN KEY REFERENCES Artists(ArtistId),
	Title nvarchar(max) NOT NULL,
	Price float NOT NULL,
	AlbumUrl nvarchar(max) NOT NULL
)
insert into Albums values
(1, 9, N'Vũ Trụ Song Song', 25.00, 'https://is1-ssl.mzstatic.com/image/thumb/Music113/v4/b6/08/14/b608145a-cbe3-452f-51c6-4732d36fec7e/190295287139.jpg/1000x1000bb.webp'),
(1, 5, N'÷ (Deluxe)', 25.00, 'https://is4-ssl.mzstatic.com/image/thumb/Features116/v4/17/0c/9d/170c9d95-1f43-eb0d-cf43-1ac6ee26877e/source/600x600bb.webp'),
(6, 3, N'Folklore', 25.00, 'https://is1-ssl.mzstatic.com/image/thumb/Video124/v4/81/02/25/810225cc-0525-24ca-31fb-0ab529b8f9e9/Job612efb24-1e81-4ea6-9fd7-dd5a8a07be22-108255586-PreviewImage_PreviewImageIntermediate_preview_image_nonvideo-Time1608057481114.png/600x600bb-60.jpg'),
(10, 7, N'Happier Than Ever', 25.00, 'https://is2-ssl.mzstatic.com/image/thumb/Features125/v4/e4/32/24/e43224c7-47f5-d32c-a046-1c508c4fb667/source/1000x1000bb.webp'),
(4, 4, 'After Hours', 25.00, 'https://is2-ssl.mzstatic.com/image/thumb/Video115/v4/f3/38/fa/f338fa6e-e347-bdf3-2ac3-6ed3401bfa25/Job299f5cf8-54d0-4d72-874c-3b5d5ee7d593-114961634-PreviewImage_preview_image_nonvideo_sdr-Time1623163436305.png/600x600bb.webp'),
(2, 2, 'Scorpion', 25.00, 'https://is5-ssl.mzstatic.com/image/thumb/Music115/v4/1f/37/43/1f374304-2e04-2be3-53ea-41dd6f0b6fb8/00602567892410.rgb.jpg/600x600bb.webp'),
(1, 6, 'V', 25.00, 'https://is2-ssl.mzstatic.com/image/thumb/Music115/v4/ac/20/32/ac203235-b03b-0e60-e03d-1ea1ee53aceb/14UMGIM31675.rgb.jpg/600x600bb.webp'),
(1, 1, 'Fine Line', 25.00, 'https://is2-ssl.mzstatic.com/image/thumb/Music115/v4/2b/c4/c9/2bc4c9d4-3bc6-ab13-3f71-df0b89b173de/886448022213.jpg/600x600bb.webp')
go
create table Orders (
	OrderId int identity(1,1) PRIMARY KEY,
	AccountId int FOREIGN KEY REFERENCES Accounts(accountId),
	OrderDate date ,
	FirstName nvarchar(100) ,
	LastName nvarchar(100) ,
	Address nvarchar(max) ,
	City nvarchar(100) ,
	State nvarchar(100) ,
	Country nvarchar(100) ,	
	Phone nvarchar(12) ,
	Total float
)
go
insert into Orders(AccountId) values (1)
go
create table OrderDetails(
	OrderDetailId int identity(1,1) PRIMARY KEY,
	OrderId int FOREIGN KEY REFERENCES Orders(OrderId),
	AlbumId int FOREIGN KEY REFERENCES Albums(AlbumId),
	Quantity int NOT NULL,
	UnitPrice float NOT NULL,
)