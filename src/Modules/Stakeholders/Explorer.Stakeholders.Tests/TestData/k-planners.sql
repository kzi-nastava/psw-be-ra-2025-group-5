INSERT INTO stakeholders."Planners"("Id", "TouristId") VALUES (-1, -21);
INSERT INTO stakeholders."PlannerDay"("Id", "Date", "PlannerId") VALUES
	(-1, '2026-01-15', -1),
	(-2, '2026-01-16', -1);
INSERT INTO stakeholders."PlannerTimeBlock"("Id", "TourId", "TimeRange", "PlannerDayId", "TransportType") VALUES
	(-1, -2, '{{"Start": "09:30:00", "End": "11:00:00"}}'::jsonb, -1, 0),
	(-2, -2, '{{"Start": "19:30:00", "End": "21:00:00"}}'::jsonb, -2, 0);
