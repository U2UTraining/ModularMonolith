In **Domain-Driven Design (DDD)**, both **Domain Events** and **Integration Events** represent things that have happened in the system. However, they serve different purposes and have different scopes:

---

### 🔹 **Domain Event**

* **Definition**: A *Domain Event* represents something that happened **within the domain** that is important to the **domain experts**.
* **Scope**: **Internal** to the **bounded context**.
* **Audience**: Other parts of the **same application** (or bounded context).
* **Example**:
  `OrderShipped` – triggered when an order changes state to shipped inside the Order domain.
* **Characteristics**:

  * Purely domain-focused.
  * Captures business intent or business facts.
  * Often used to **trigger internal logic** (e.g., sending a confirmation email, updating aggregates).
  * Typically handled **synchronously or asynchronously** within the same service or module.

---

### 🔸 **Integration Event**

* **Definition**: An *Integration Event* is used to **communicate state changes across bounded contexts or services**—often in a distributed system.
* **Scope**: **External** – between **bounded contexts** or **microservices**.
* **Audience**: Other **services, systems**, or **external consumers**.
* **Example**:
  `OrderShipped` – published to a message bus so the **Shipping service** or **Billing service** can react.
* **Characteristics**:

  * Focused on **communication between systems**.
  * Often handled via **message brokers** (e.g., RabbitMQ, Azure Service Bus, Kafka).
  * Tend to be more **stable** and versioned carefully since they cross service boundaries.
  * Usually **asynchronous**.

---

### ✅ Summary Table

| Feature              | Domain Event                      | Integration Event                   |
| -------------------- | --------------------------------- | ----------------------------------- |
| **Scope**            | Internal (within bounded context) | External (across contexts/services) |
| **Audience**         | Domain logic                      | Other systems or services           |
| **Used for**         | Business rules, workflows         | System-to-system communication      |
| **Transport**        | In-process (sync/async)           | Message bus / pub-sub / event bus   |
| **Stability needed** | Low (internal use)                | High (public contract)              |

---

### 🧩 Practical Relationship

In many systems, **Domain Events can lead to Integration Events**.

For example:

* A `PaymentReceived` domain event inside the Payments bounded context
* → triggers the publication of an `InvoicePaid` integration event to notify other systems

---

Would you like a C# example to illustrate the difference?
