CREATE OR REPLACE FUNCTION profile_guard() RETURNS TRIGGER AS $profile_guard$
	DECLARE
		same_type_violation_report_count_on_post INTEGER;
	BEGIN
		IF NOT NEW.first_name ~'[A-Za-z]+' THEN
			RAISE EXCEPTION 'Only letters allowed in first name';
		END IF;
		IF NOT NEW.last_name ~'[A-Za-z]+' THEN
			RAISE EXCEPTION 'Only letters allowed in last name';
		END IF;
		IF NOT NEW.avatar_url ~'.+\.(png|jpg)' THEN
			RAISE EXCEPTION 'Wrong avatar format';
		END IF;
		IF NEW.role_id IS NULL THEN
			NEW.role_id = 1;
		END IF;
		RETURN NEW;
	END;
$profile_guard$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER profile_guard
BEFORE INSERT OR UPDATE ON user_profiles FOR EACH ROW EXECUTE PROCEDURE autodelete_post();