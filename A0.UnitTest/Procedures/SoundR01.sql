/*
@SoundFilePath = null
*/
SELECT * 
FROM Sound 
WHERE (@SoundFilePath IS NULL OR SoundFilePath = @SoundFilePath)