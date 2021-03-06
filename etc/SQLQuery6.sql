
  DECLARE @campground_id int = 3
  DECLARE @park_id int = 1
  DECLARE @from_date DATETIME = '2018-02-27 13:13:07.440'
  DECLARE @to_date DATETIME = '2018-03-28 13:13:07.440'
  DECLARe @MONTHFROM INT = MONTH(@from_date) 
  DECLARe @MONTHto INT = MONTH(@to_date) 
  
  Select  reservation.*
  from site 
  Join reservation ON site.site_id = reservation.site_id 
  JOIN campground ON campground.campground_id = site.campground_id 
  JOIN park on park.park_id = campground.park_id
  Where site.campground_id = @campground_id 
  AND campground.park_id = @park_id
  AND @MONTHFROM BETWEEN campground.open_from_mm AND campground.open_to_mm
  AND @MONTHto BETWEEN campground.open_from_mm and campground.open_to_mm
  


   DECLARE @campground_id int = 3
  DECLARE @park_id int = 1
  DECLARE @from_date DATETIME = '2018-02-27 13:13:07.440'
  DECLARE @to_date DATETIME = '2018-03-15 13:13:07.440'
  DECLARe @MONTHFROM INT = MONTH(@from_date) 
  DECLARe @MONTHto INT = MONTH(@to_date) 
  
  Select  *
  from reservation
  Join site ON reservation.site_id = site.site_id
  JOIN campground on campground.campground_id = site.campground_id 
  JOIN park on park.park_id = campground.park_id
  Where site.campground_id = @campground_id 
  AND campground.park_id = @park_id
  
  AND reservation.to_date 
  <=  @from_date 

  AND reservation.from_date BETWEEN @to_date and @from_date 
  
  AND campground.open_from_mm <=  @MONTHFROM
  AND campground.open_from_mm <= @MONTHto 
  AND campground.open_to_mm > = @MONTHto 
  AND campground.open_to_mm > =  @MONTHFROM 
  GROUP BY site.campground_id, site.site_id