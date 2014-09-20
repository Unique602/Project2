using System; 
using System.Threading; 

namespace ConsoleAppPriceCut {

public delegate void priceCutEvent(Int32 pr); // Define a delegate
 
public class ChickenFarm {
    static Random rng = new Random(); // To generate random numbers 
    public static event priceCutEvent priceCut; // Link event to delegate 
    private static Int32 chickenPrice = 10; 

    public Int32 getPrice() 
        {
           return chickenPrice;
        } 
    public static void changePrice(Int32 price) 
        {
            if (price < chickenPrice) { // a price cut 
                if (priceCut != null) // there is at least a subscriber
                    priceCut(price);	// emit event to subscribers
        } 
    chickenPrice = price;
} 

 public void farmerFunc() {
        for (Int32 i = 0; i < 50; i++) 
        { 
            Thread.Sleep(500); 
            // Take the order from the queue of the orders; 
            // Decide the price based on the orders 
            Int32 p = rng.Next(5, 10); //generates price from 5, 10 every 500milliseconds
            // Console.WriteLine("New Price is {0}", p); 
            ChickenFarm.changePrice(p);
        }
} 

    public class Retailer {

        public void retailerFunc() //for starting thread
        {	
            ChickenFarm chicken = new ChickenFarm(); 
            for (Int32 i = 0; i < 10; i++) {
                Thread.Sleep(1000); //checks the price every 1000 milliseconds
                Int32 p = chicken.getPrice(); 
                Console.WriteLine("Store{0} has everyday low price: ${1} each",
                    Thread.CurrentThread.Name, p); 
                // Thread.CurrentThread.Name prints thread name 
            }
        }

        public void chickenOnSale(Int32 p) { // Event handler
            // order chickens from chicken farm – send order into queue 
            Console.WriteLine("Store{0} chickens are on sale: as low as ${1} each",
                    Thread.CurrentThread.Name, p); 
                // Thread.CurrentThread.Name cannot print a name 
        }
}

    public class OrderObject
    {
        //  senderId: the identity of the sender, you can use thread name or thread id;
        //	cardNo: a long integer that represents a credit card number;
        //	amount: an integer that represents the number of chickens to order;
        //	setID and getID: methods allow the users to write and read senderId member
        //	setCardNo and getCardNo: methods allow the users to write and read cardNo member
        //	setAmt and getAmt: methods allow the users to write and read Amount member
        //
        //
    }

    public class BufferObject
    {
        //one data member of OrderObject class
        // two methods: getOrderObject() and setOrderObject()
    }

    public class BufferString //similar to BufferObject, except containing a string as data
    {
        // two methods: getOrderObject() and setOrderObject() ??
    }

    public class EncoderDecoder // should this be split into two different methods (Encoder)(Decoder)????
    {
        //two service operations: Encoding and decoding
        //implement encryption and decryption
    }
    /*
     *Retailer1 through RetailerN, each is a thread instantiated from the same class 
     *or a method in the class, for example, N = 5. In each retailer thread, a loop is 
     *used to generate m (e.g., m = 10) orders. Each order is an OrderObject class object. 
     *The object is sent to the encoder/decoder threads for encoding. The encoded string 
     *is sent back to the retailer. Different ways are possible; for example, one can define 
     *a one-cell buffer with a getID method. A retailer reads the string if the ID matches. 
     *Then, the string is sent to the chicken farm thread through a multicell buffer. 
     *A semaphore can be used to manage the cells. Each retailer thread will print a list of 
     *human-friendly output (orders placed). 
     */

    public class MultiCellBuffer
    {
        /*
         *  This class has n data cells. The number n of cells is smaller than the 
         *  number of retailers N. A setOneCell and a getOneCell methods can be defined 
         *  to write and to read the data. A semaphore of value n can be used to manage the 
         *  cells.
         */
    }

    /*
     * Main: The Main thread will perform necessary preparation, 
     * create the buffer classes, 
     * instantiate the objects, 
     * create threads, 
     * and start threads
     */

    //Solutions for deadlock?? Read the lectures

    /*
     * A deadlock can occur in such a complex multithreading program 
     * if the threads and shared resources are not properly planned and 
     * designed; for example, if one creates three objects to transfer the 
     * three pieces of data: senderID, cardNo, and amount, respectively, and 
     * different retailers can hold (lock) an object while waiting for the second 
     * object, or holding two objects and waiting for the third object, a deadlock 
     * can occur. Figure 2.30 shows a design where deadlock can occur.
     * 
     * That a deadlock can occur does not mean a deadlock will occur every time. 
     * It also depends on timing. This situation actually makes things worse, as 
     * you may not detect the deadlock possibility during the test phase.
     */

    public class myApplication
        {
            static void Main(string[] args)
            {
                ChickenFarm chicken = new ChickenFarm(); //caller is in the chickenFarmer, but function resides in the Retailer
                Thread farmer = new Thread(new ThreadStart(chicken.farmerFunc)); 
                farmer.Start();	// Start one farmer thread 

                Retailer chickenstore = new Retailer(); 
                ChickenFarm.priceCut += new priceCutEvent(chickenstore.chickenOnSale); 
                //registers the chickOnSale() method (event handler in all the Retailers 
                //  to the priceCut event in the ChickenFarm thread
                //Since priceCut event is defined as a delegate, that can call all the 
                //  subscribed event handlers when the event occurs
                //The changePrice() method searches if there is a subscriber by priceCut!=NULL
                
                //eventhandler is only used when an event happens (or there is a price change)
 
                Thread[] retailers = new Thread[3];
                for (int i = 0; i < 3; i++) //N= 3here  //creates stores 1,2,3
                {	// Start N retailer threads

                    retailers[i] = new Thread(new ThreadStart(chickenstore.retailerFunc));
                    retailers[i].Name = (i + 1).ToString(); //makes names for retailers(Threads)
                    retailers[i].Start(); //start thread
            }
        }
    }


}
}
