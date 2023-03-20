namespace tomoyo.Emulator
{
    /// <summary>
    /// Simple interface that provides methods for reading from, writing to, and checking the existence of addresses in an address space.
    /// </summary>
    public interface IAddressable
    {
        /// <summary>
        /// Checks to see if the underlying address space contains the given address.
        /// </summary>
        /// <param name="addr">The address to check.</param>
        /// <returns>Whether or not the address exists in the address space.</returns>
        public bool Contains(long addr);
        /// <summary>
        /// Retrieves a value from the underlying address space.
        /// </summary>
        /// <param name="addr">The address to read.</param>
        /// <returns>The word at the given address within the underlying address space.</returns>
        public long Read(long addr);
        /// <summary>
        /// Writes a value to the memory at the given address in the underlying address space.
        /// </summary>
        /// <param name="addr">The address to write to.</param>
        /// <param name="val">The value to write.</param>
        /// <returns></returns>
        public long Write(long addr, long val);
    }
}