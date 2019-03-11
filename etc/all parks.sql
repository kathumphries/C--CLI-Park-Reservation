
Declare @park_id int  = 2 
DECLARE @from_date DATETIME = '2019-03-20 13:13:07.440'
DECLARE @to_date DATETIME = '2019-03-20 13:13:07.440'



(SELECT *
FROM site
JOIN reservation on reservation.site_id = SITE.site_id
JOIN campground on campground.campground_id = site.campground_id
JOIN park ON PARK.park_id = campground.park_id
Where park.park_id = @park_id
AND reservation.from_date NOT BETWEEN @from_date AND @to_date
AND reservation.to_date NOT BETWEEN @from_date AND @to_date
AND campground.open_from_mm <= MONTH(from_date) 
AND campground.open_from_mm <= MONTH(to_date)
AND campground.open_to_mm > =  MONTH(from_date)
AND campground.open_to_mm > = MONTH(to_date))



Declare @park_id int  = 2 
DECLARE @from_date DATETIME = '2019-03-20 13:13:07.440'
DECLARE @to_date DATETIME = '2019-03-20 13:13:07.440'

Select *
FROM
(SELECT site.*, row_number() over(partition by site.campground_id order by site.site_number) as rn
FROM site
JOIN reservation on reservation.site_id = SITE.site_id
JOIN campground on campground.campground_id = site.campground_id
JOIN park ON PARK.park_id = campground.park_id
Where park.park_id = @park_id
AND reservation.from_date NOT BETWEEN @from_date AND @to_date
AND reservation.to_date NOT BETWEEN @from_date AND @to_date
AND campground.open_from_mm <= MONTH(from_date) 
AND campground.open_from_mm <= MONTH(to_date)
AND campground.open_to_mm > =  MONTH(from_date)
AND campground.open_to_mm > = MONTH(to_date)
) as T
WHERE T.rn <=5


