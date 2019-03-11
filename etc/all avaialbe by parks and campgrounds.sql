
DECLARE @campground_id int = 3
  DECLARE @park_id int = 1
  DECLARE @from_date DATETIME = '2019-02-27 13:13:07.440'
  DECLARE @to_date DATETIME = '2019-03-28 13:13:07.440'
 DECLARe @fromMonth INT = 2
  DECLARe @toMonth INT = 3

SELECT  site.*
FROM site
JOIN campground on campground.campground_id = site.campground_id
WHERE campground.campground_id = @campground_id
AND @fromMonth BETWEEN campground.open_from_mm AND campground.open_to_mm
AND @toMonth BETWEEN campground.open_from_mm AND campground.open_to_mm
AND site.site_id NOT IN 
(SELECT reservation.site_id FROM reservation WHERE @from_date BETWEEN from_date AND to_date
Union
SELECT reservation.site_id FROM reservation WHERE @to_date BETWEEN from_date AND to_date
Union
SELECT reservation.site_id FROM reservation WHERE @from_date <= from_date AND @to_date >= to_date) 