/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [reservation_id]
      ,[site_id]
      ,[name]
      ,[from_date]
      ,[to_date]
      ,[create_date]
  FROM [NationalParkReservation].[dbo].[reservation]

  DECLARE @campground_id int = 3
  DECLARE @park_id int = 1
  DECLARE @from_date DATETIME = '2018-02-27 13:13:07.440'
  DECLARE @to_date DATETIME = '2018-03-01 13:13:07.440'
  DECLARe @MONTHFROM INT = MONTH(@from_date) 
  DECLARe @MONTHto INT = MONTH(@to_date) 
  
  Select  site.site_id, site.campground_id 
  from site 
  Join reservation ON site.site_id = reservation.site_id 
  JOIN campground ON campground.campground_id = site.campground_id 
  JOIN park on park.park_id = campground.park_id
  Where site.campground_id = @campground_id 
  AND campground.park_id = @park_id
  AND campground.open_from_mm <=  @MONTHFROM
  AND campground.open_from_mm <= @MONTHto 
  AND campground.open_to_mm > = @MONTHto 
  AND campground.open_to_mm > =  @MONTHFROM 
   AND reservation.from_date <= @to_date 
  AND reservation.to_date >=  @from_date 
  GROUP BY site.campground_id, site.site_id
  


  The short(ish) answer is: given two date intervals A and B with components .start and .end and the constraint .start <= .end, then two intervals overlap if:

A.end >= B.start AND A.start <= B.end
  
  