public class NotIsTheirChildException : Exception {
    public NotIsTheirChildException(int parentId, int childId) : base(String.Format("The child with id {0} is not child of the parent with id {1}", childId, parentId))
    {
        
    }
}