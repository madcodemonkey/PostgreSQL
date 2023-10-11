namespace Example.Model;

/// <summary>
/// When you do multiple searches on different vectors, this class is used to combine them before doing
/// an in memory comparision.  Why?  You can't compare two vector fields to query vector.
/// </summary>
/// <remarks>
/// Note 1:  If you only have one vector field in your record, you will NOT need this class.
/// Note 2:  If you only need to query ONE of the vector fields in your record, you will NOT need this class.
/// Note 3:  As stated above, if you query twice on different vectors trying to find a field with the best answer, you WILL use this class.
/// </remarks>
public class OneVectorAndData<T> where T : class
{
    /// <summary>
    /// The vector from one of the fields in Data
    /// </summary>
    public float[] TheVector { get; set; }

    /// <summary>
    /// The original data.
    /// </summary>
    public T Data { get; set; }
}